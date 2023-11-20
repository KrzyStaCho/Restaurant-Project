﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Helper;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel
{
    public class SupplierVM : BaseViewModel
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        private List<SupplierModel> _supply;
        private string searchbox;

        #endregion
        #region Public Fields

        #endregion
        #region Data Binding

        public List<SupplierModel> SupplyList
        {
            get { return _supply; }
            set
            {
                _supply = value;
                OnPropertyChanged(nameof(SupplyList));
            }
        }
        public string SearchBox
        {
            get { return searchbox; }
            set
            {
                searchbox = value;
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
            LoadData(string.Empty);
        }
        private void ExecuteSearchData(object? parameter)
        {
            LoadData(SearchBox);
        }
        private void ExecuteAddSupplier(object? parameter)
        {
            //TODO: Add supplier
        }
        private void ExecuteEditSupplier(object? parameter)
        {
            //Validate parameter
            if (!(parameter is SupplierModel)) return;
            SupplierModel model = (SupplierModel)parameter;

            //TODO: Edit Supplier
        }
        private void ExecuteDeleteSupplier(object? parameter)
        {
            //Validate parameter
            if (!(parameter is SupplierModel)) return;
            SupplierModel model = (SupplierModel)parameter;

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that supplier?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            //Remove supplier
            Supplier targetSupplier = new Supplier() { SupplierId = model.SupplierID };
            EntityEntry<Supplier> entityEntry = database.Suppliers.Attach(targetSupplier);
            entityEntry.State = EntityState.Deleted;

            //Save changes
            database.SaveChanges();
            RefreshData.Execute(null);
        }

        private bool CanExecuteModifyData(object? parameter)
        {
            if (mainVM.ActiveUser == null) return false;
            return mainVM.ActiveUser.HasPerm(ProgramCode.ModifySupplierCode);
        }

        #endregion
        #region Functions

        private void LoadData(string searching)
        {
            SearchBox = string.Empty;
            SupplyList = ModelConstructor.GetSuppliers(database, searching);
        }

        #endregion

        public SupplierVM()
        {
            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;

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