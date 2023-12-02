using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model;
using StorageApplication.MVVM.Model.DataGrid;
using StorageApplication.MVVM.Model.Form;
using StorageApplication.MVVM.View.WindowForm;
using StorageApplication.MVVM.ViewModel.WindowForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel
{
    class SupplierVM : BaseChildViewModel
    {
        #region Private Fields

        private List<SupplierModel> suppliers;
        private string searchBox;

        #endregion
        #region Public Fields
        #endregion
        #region Data Binding

        public List<SupplierModel> SupplierList
        {
            get { return suppliers; }
            set
            {
                suppliers = value;
                OnPropertyChanged(nameof(SupplierList));
            }
        }
        public string SearchBox
        {
            get { return searchBox; }
            set
            {
                searchBox = value;
                OnPropertyChanged(nameof(SearchBox));
            }
        }

        #endregion
        #region Commands

        public ICommand RefreshData { get; }
        public ICommand SearchData { get; }
        public ICommand AddSupplier { get; }
        public ICommand EditSupplier { get; }
        public ICommand DeleteSupplier { get; }

        private void ExecuteRefreshData(object? parameter)
        {
            SearchBox = string.Empty;
            LoadData();
        }
        private void ExecuteSearchData(object? parameter)
        {
            LoadData(SearchBox);
        }
        private void ExecuteAddSupplier(object? parameter)
        {
            Window addWindow = mainVM.SetOwner(new WindowSupplierForm());
            WindowSupplierVM addSupplierVM = new WindowSupplierVM(addWindow);

            addWindow.ShowDialog();

            //Check if user confirm changes
            if (addSupplierVM.ActionState != FormStateEnum.CONFIRM) return;

            //Add user data to new supplier
            Supplier toAddSupplier = addSupplierVM.FormModel.GetInput();
            toAddSupplier.WhoChanged = mainVM.ActiveUsername;
            toAddSupplier.LastModified = DateTime.Now.Date;

            //Add supplier to DB
            database.Suppliers.Add(toAddSupplier);
            database.SaveChanges();
            mainVM.IncrementChangesInDB();

            //Refresh data
            RefreshData.Execute(null);
        }
        private void ExecuteEditSupplier(object? parameter)
        {
            //Validate parameter
            if (parameter is not SupplierModel) return;
            SupplierModel model = (SupplierModel)parameter;

            //Get raw supplier model
            Supplier? rawModel = database.Suppliers.Find(model.SupplierID);
            if (rawModel == null) return;

            Window editWindow = mainVM.SetOwner(new WindowSupplierForm());
            WindowSupplierVM editSupplierVM = new WindowSupplierVM(editWindow, rawModel);

            editWindow.ShowDialog();

            //Check if user confirm changes
            if (editSupplierVM.ActionState != FormStateEnum.CONFIRM) return;

            //Add user data to supplier
            Supplier toEditSupplier = editSupplierVM.FormModel.GetInput();
            toEditSupplier.WhoChanged = mainVM.ActiveUsername;
            toEditSupplier.LastModified = DateTime.Now.Date;

            //Save changes
            database.SaveChanges();
            mainVM.IncrementChangesInDB();
            database.Suppliers.Entry(toEditSupplier).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            //Refresh data
            RefreshData.Execute(null);
        }
        private void ExecuteDeleteSupplier(object? parameter)
        {
            //Validate parameter
            if (parameter is not SupplierModel) return;
            SupplierModel model = (SupplierModel)parameter;

            //Get raw supplier model
            Supplier? toRemoveSupplier = database.Suppliers.Find(model.SupplierID);
            if (toRemoveSupplier == null) return;

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that supplier?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            //Add user data to supplier
            toRemoveSupplier.WhoChanged = mainVM.ActiveUsername;
            toRemoveSupplier.LastModified = DateTime.Now.Date;

            DBModelConstructor.RemoveSupplier(database, toRemoveSupplier);
            mainVM.IncrementChangesInDB();

            //Refresh data
            RefreshData.Execute(null);
        }

        private bool CanExecuteModifyData(object? parameter)
        {
            if (mainVM.ActiveUser == null) return false;
            return mainVM.ActiveUser.HasPerm(ProgramCode.ModifySupplierCode);
        }

        #endregion
        #region Functions

        private void LoadData(string searchText = "")
        {
            List<Supplier> rawSuppliers = DBModelConstructor.GetSuppliers(database, searchText);
            database.ChangeTracker.Clear();

            SupplierList = rawSuppliers.Select(sp => new SupplierModel(sp)).ToList();
        }

        #endregion

        public SupplierVM()
        {
            #region Init Commands

            RefreshData = new BaseCommand(ExecuteRefreshData);
            SearchData = new BaseCommand(ExecuteSearchData);
            AddSupplier = new BaseCommand(ExecuteAddSupplier, CanExecuteModifyData);
            EditSupplier = new BaseCommand(ExecuteEditSupplier, CanExecuteModifyData);
            DeleteSupplier = new BaseCommand(ExecuteDeleteSupplier, CanExecuteModifyData);

            #endregion

            RefreshData.Execute(null);
        }
    }
}
