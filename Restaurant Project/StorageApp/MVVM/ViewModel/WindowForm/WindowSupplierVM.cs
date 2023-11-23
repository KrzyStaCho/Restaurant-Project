﻿using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.Model.Form;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel.WindowForm
{
    public class WindowSupplierVM : BaseWindowVM
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public bool IsConfirmed;

        #endregion
        #region Data Binding

        public SupplierForm DataForm { get; private set; }

        #endregion
        #region Commands

        public ICommand ConfirmData { get; }

        private void ExecuteConfirmData(object? parameter)
        {
            if (!DataForm.CheckData()) return;
            DataForm.TrimData();
            IsConfirmed = true;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            return !(DataForm.IsDataEmpty());
        }

        #endregion
        #region Functions

        #endregion

        public WindowSupplierVM(Window window, Window owner, SupplierModel? model = null) : base(window, "")
        {
            WindowTitle = (model == null) ? "Add Supplier" : "Edit Supplier";
            window.Owner = owner;

            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;
            IsConfirmed = false;

            #region Init Commands

            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);

            #endregion

            DataForm = new SupplierForm();
            if (model != null)
            {
                Supplier? rawModel = database.Suppliers.FirstOrDefault(sp => (sp.SupplierId == model.SupplierID));
                if (rawModel == null) return;

                DataForm.FillData(rawModel);
            }
        }
    }
}
