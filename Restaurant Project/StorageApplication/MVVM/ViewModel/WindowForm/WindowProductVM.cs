using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.DataGrid;
using StorageApplication.MVVM.Model.Form;
using StorageApplication.MVVM.Model.Item;
using StorageApplication.MVVM.View.WindowSelect;
using StorageApplication.MVVM.ViewModel.WindowSelect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowForm
{
    class WindowProductVM : BaseWindowVM
    {
        #region Private Fields

        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public FormStateEnum FormState;

        #endregion
        #region Data Binding

        public ProductForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand GetCategory { get; }
        public ICommand GetSupplier { get; }
        public ICommand ConfirmData { get; }

        private void ExecuteGetCategory(object? parameter)
        {
            Window categoryWindow = new WindowProductCategorySelect();
            categoryWindow.Owner = mainWindow;
            BaseWindowSelectVM<ObjectPair<int, string>> categoryVM = new BaseWindowSelectVM<ObjectPair<int, string>>(categoryWindow, "Choose category", LoadCategory);

            categoryWindow.ShowDialog();

            //Check if user confirm
            ObjectPair<int, string>? model = categoryVM.SelectedObject;
            if (model == null) return;

            //Set category
            FormModel.SelectedCategory = model;
        }
        private void ExecuteGetSupplier(object? parameter)
        {
            Window supplierWindow = new WindowSupplierSelect();
            supplierWindow.Owner = mainWindow;
            BaseWindowSelectVM<SupplierModel> supplierVM = new BaseWindowSelectVM<SupplierModel>(supplierWindow, "Choose supplier", LoadSupplier);

            supplierWindow.ShowDialog();

            //Check if user confirm
            SupplierModel? model = supplierVM.SelectedObject;
            if (model == null) return;

            //Set supplier
            FormModel.SelectedSupplier = new ObjectPair<int, string>(model.SupplierID, model.CompanyName);
        }
        private void ExecuteConfirmData(object? parameter)
        {
            if (!CanExecuteConfirmData(null)) return;
            FormState = FormStateEnum.CONFIRM;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            return FormModel.IsDataCorrect();
        }

        #endregion
        #region Functions
        
        private List<ObjectPair<int, string>> LoadUnits()
        {
            List<MeasureUnit> units = DBModelConstructor.GetMeasureUnits(database);

            return units.Select(u => new ObjectPair<int, string>(u.UnitId, u.Code)).ToList();
        }
        private List<SupplierModel> LoadSupplier(string searching)
        {
            List<Supplier> rawSuppliers = DBModelConstructor.GetSuppliers(database, searching);

            return rawSuppliers.Select(sp => new SupplierModel(sp)).ToList();
        }
        private List<ObjectPair<int, string>> LoadCategory(string searching)
        {
            List<ProductCategory> rawCategory = DBModelConstructor.GetProductCategories(database, searching);

            return rawCategory.Select(pc => new ObjectPair<int, string>(pc.CategoryId, pc.CategoryName)).ToList();
        }

        #endregion

        public WindowProductVM(Window window, Product? product = null)
            : base(window, (product == null) ? "Add Product" : "Edit Product")
        {
            database = MainWindowVM.DatabaseInstance;
            FormState = FormStateEnum.CANCEL;

            #region Init Commands

            GetCategory = new BaseCommand(ExecuteGetCategory);
            GetSupplier = new BaseCommand(ExecuteGetSupplier);
            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);

            #endregion

            FormModel = new ProductForm(LoadUnits(), GetCategory, GetSupplier);
            if (product != null)
            {
                FormModel.FillInput(product);
            }
        }
    }
}
