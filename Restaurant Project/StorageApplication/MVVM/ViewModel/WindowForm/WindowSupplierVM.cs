using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Model.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowForm
{
    class WindowSupplierVM : BaseWindowVM
    {
        #region Private Fields
        #endregion
        #region Public Fields

        public FormStateEnum ActionState { get; private set; }

        #endregion
        #region Data Binding

        public SupplierForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand ConfirmAction { get; }

        private void ExecuteConfirmAction(object? parameter)
        {
            if (!FormModel.IsDataCorrect()) return;
            ActionState = FormStateEnum.CONFIRM;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmAction(object? parameter)
        {
            return FormModel.IsDataCorrect();
        }

        #endregion
        #region Functions
        #endregion

        public WindowSupplierVM(Window window, Supplier? model = null)
            : base(window, (model == null) ? "Add Supplier" : "Edit Supplier")
        {
            ActionState = FormStateEnum.CANCEL;

            #region Init Commands

            ConfirmAction = new BaseCommand(ExecuteConfirmAction, CanExecuteConfirmAction);

            #endregion

            FormModel = new SupplierForm();
            if (model != null)
            {
                FormModel.FillInput(model);
            }
        }
    }
}
