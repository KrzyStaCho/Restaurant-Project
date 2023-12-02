using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.Form;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowForm
{
    class WindowMeasureUnitVM : BaseWindowVM
    {
        #region Private Fields

        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public FormStateEnum FormState;

        #endregion
        #region Data Binding

        public MeasureUnitForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand ConfirmData { get; }
        public ICommand DeleteData { get; }

        private void ExecuteConfirmData(object? parameter)
        {
            if (!CanExecuteConfirmData(null)) return;

            FormState = (FormModel.SelectedItem?.FirstItem == -1) ? FormStateEnum.CREATE : FormStateEnum.EDIT;
            CloseCommand.Execute(null);
        }
        private void ExecuteDeleteData(object? parameter)
        {
            if (!CanExecuteDeleteData(null)) return;

            if(FormModel.SelectedItem?.FirstItem == -1)
            {
                FormModel.Error = "Cannot delete unit that hasn't created yet!";
                return;
            }

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that unit?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            FormState = FormStateEnum.DELETE;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            return FormModel.IsDataCorrect();
        }
        private bool CanExecuteDeleteData(object? paramter)
        {
            return (FormModel.SelectedItem != null);
        }

        #endregion
        #region Functions

        private List<ObjectPair<int, string>> LoadUnits()
        {
            List<MeasureUnit> units = DBModelConstructor.GetMeasureUnits(database);
            List<ObjectPair<int, string>> unitList = units.Select(u => new ObjectPair<int, string>(u.UnitId, u.Code)).ToList();
            unitList.Add(new ObjectPair<int, string>(-1, "+ New unit"));

            database.ChangeTracker.Clear();

            return unitList;
        }
        private void LoadUnit(int unitId)
        {
            if (unitId == -1) return;

            MeasureUnit? unit = database.MeasureUnits.Find(unitId);
            if (unit == null) return;

            FormModel.FillInput(unit);
        }

        #endregion

        public WindowMeasureUnitVM(Window window)
            : base(window, "Modify measure unit")
        {
            database = MainWindowVM.DatabaseInstance;
            FormState = FormStateEnum.CANCEL;

            #region Init Commands

            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);
            DeleteData = new BaseCommand(ExecuteDeleteData, CanExecuteDeleteData);

            #endregion

            FormModel = new MeasureUnitForm(LoadUnits(), LoadUnit);
        }
    }
}
