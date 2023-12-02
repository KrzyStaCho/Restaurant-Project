using MaterialDesignThemes.Wpf;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model;
using StorageApplication.MVVM.Model.DataGrid;
using StorageApplication.MVVM.Model.Item;
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
    class ProductVM : BaseChildViewModel
    {
        #region Private Fields

        private List<ObjectPair<int, string>> _categoryMenu;
        private List<ProductModel> _productList;
        private string searchBox;

        #endregion
        #region Public Fields
        #endregion
        #region Data Binding

        public List<ObjectPair<int, string>> CategoryList
        {
            get { return _categoryMenu; }
            set
            {
                _categoryMenu = value;
                OnPropertyChanged(nameof(CategoryList));
            }
        }
        public List<ProductModel> ProductList
        {
            get { return _productList; }
            set
            {
                _productList = value;
                OnPropertyChanged(nameof(ProductList));
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
        public ICommand FromCategoryData { get; }
        public ICommand AddProduct { get; }
        public ICommand EditProduct { get; }
        public ICommand DeleteProduct { get; }
        public ICommand ModifyUnit { get; }
        public ICommand ModifyCategory { get; }

        private void ExecuteRefreshData(object? parameter)
        {
            SearchBox = string.Empty;
            LoadFullData();
        }
        private void ExecuteSearchData(object? parameter)
        {
            LoadProducts(SearchBox);
        }
        private void ExecuteFromCategoryData(object? parameter)
        {
            if (parameter is not int) return;
            int categoryId = (int)parameter;

            LoadProductsByCategory(categoryId);
        }
        private void ExecuteAddProduct(object? parameter)
        {
            Window addWindow = mainVM.SetOwner(new WindowProductForm());
            WindowProductVM addProductVM = new WindowProductVM(addWindow);

            addWindow.ShowDialog();

            //Check if user confirm changes
            if (addProductVM.FormState != Model.Form.FormStateEnum.CONFIRM)
                return;

            //Add user info to model
            Product toAddProduct = addProductVM.FormModel.GetInput();
            toAddProduct.WhoChanged = mainVM.ActiveUsername;
            toAddProduct.LastModified = DateTime.Now.Date;

            //Add product to DB
            database.Products.Add(toAddProduct);
            database.SaveChanges();
            mainVM.IncrementChangesInDB();
            database.ChangeTracker.Clear();

            //Refresh view
            RefreshData.Execute(null);

        }
        private void ExecuteEditProduct(object? parameter)
        {
            //Validate parameter
            if (parameter is not ProductModel) return;
            ProductModel model = (ProductModel)parameter;

            //Get raw model from DB
            Product? rawModel = DBModelConstructor.GetProduct(database, model.ProductID);
            if (rawModel == null) return;

            Window editWindow = mainVM.SetOwner(new WindowProductForm());
            WindowProductVM editProductVM = new WindowProductVM(editWindow, rawModel);

            editWindow.ShowDialog();

            //Check if user confirm changes
            if (editProductVM.FormState != Model.Form.FormStateEnum.CONFIRM)
                return;

            //Add user info to model
            Product toEditProduct = editProductVM.FormModel.GetInput();
            toEditProduct.WhoChanged = mainVM.ActiveUsername;
            toEditProduct.LastModified = DateTime.Now.Date;

            //Save changes
            database.SaveChanges();
            mainVM.IncrementChangesInDB();
            database.Products.Entry(toEditProduct).State = Microsoft.EntityFrameworkCore.EntityState.Detached;

            //Refresh view
            RefreshData.Execute(null);
        }
        private void ExecuteDeleteProduct(object? parameter)
        {
            //Validate parameter
            if (parameter is not ProductModel) return;
            ProductModel model = (ProductModel)parameter;

            //Get raw product model
            Product? toRemoveProduct = DBModelConstructor.GetProduct(database, model.ProductID);
            if (toRemoveProduct == null) return;

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that product?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            //Add user data to product
            toRemoveProduct.WhoChanged = mainVM.ActiveUsername;
            toRemoveProduct.LastModified = DateTime.Now.Date;

            DBModelConstructor.RemoveProduct(database, toRemoveProduct);
            mainVM.IncrementChangesInDB();

            //Refresh data
            RefreshData.Execute(null);
        }
        private void ExecuteModifyUnit(object? parameter)
        {
            Window modifyWindow = mainVM.SetOwner(new WindowMeasureUnitForm());
            WindowMeasureUnitVM modifyUnitVM = new WindowMeasureUnitVM(modifyWindow);

            modifyWindow.ShowDialog();

            //Check if user confirm changes and do target action
            switch(modifyUnitVM.FormState)
            {
                case Model.Form.FormStateEnum.CREATE:
                    AddUnit(modifyUnitVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.EDIT:
                    EditUnit(modifyUnitVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.DELETE:
                    DeleteUnit(modifyUnitVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.CANCEL:
                default:
                    return;
            }

            //Refresh view
            LoadProducts(string.Empty);
        }
        private void ExecuteModifyCategory(object? parameter)
        {
            Window modifyWindow = mainVM.SetOwner(new WindowProductCategoryForm());
            WindowProductCategoryVM modifyCategoryVM = new WindowProductCategoryVM(modifyWindow);

            modifyWindow.ShowDialog();

            //Check if user confirm changes and do target action
            switch(modifyCategoryVM.FormState)
            {
                case Model.Form.FormStateEnum.CREATE:
                    AddCategory(modifyCategoryVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.EDIT:
                    EditCategory(modifyCategoryVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.DELETE:
                    DeleteCategory(modifyCategoryVM.FormModel.GetInput());
                    break;
                case Model.Form.FormStateEnum.CANCEL:
                default:
                    return;
            }
            
            //Refresh view
            LoadFullData();
        }

        private bool CanExecuteModifyData(object? parameter)
        {
            if (mainVM.ActiveUser == null) return false;
            return mainVM.ActiveUser.HasPerm(ProgramCode.ModifyProductCode);
        }

        #endregion
        #region Functions

        private void LoadFullData()
        {
            LoadCategoryMenu();
            LoadProducts(string.Empty);
        }
        private void LoadProducts(string searching)
        {
            List<Product> products = DBModelConstructor.GetProducts(database, searching);
            database.ChangeTracker.Clear();

            ProductList = products.Select(p => new ProductModel(p)).ToList();
        }
        private void LoadProductsByCategory(int categoryId)
        {
            List<Product> products = DBModelConstructor.GetProductsByCategory(database, categoryId);
            database.ChangeTracker.Clear();

            ProductList = products.Select(p => new ProductModel(p)).ToList();
        }
        private void LoadCategoryMenu()
        {
            CategoryList = DBModelConstructor.GetCategoryName(database);
        }

        #region Modify Category Help Function

        private void AddCategory(ProductCategory category)
        {
            database.ProductCategories.Add(category);
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void EditCategory(ProductCategory category)
        {
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void DeleteCategory(ProductCategory category)
        {
            DBModelConstructor.RemoveCategory(database, category);
            mainVM.IncrementChangesInDB();
        }

        #endregion
        #region Modify Unit Help Function

        private void AddUnit(MeasureUnit unit)
        {
            database.MeasureUnits.Add(unit);
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void EditUnit(MeasureUnit unit)
        {
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void DeleteUnit(MeasureUnit unit)
        {
            DBModelConstructor.RemoveMeasureUnit(database, unit);
            mainVM.IncrementChangesInDB();
        }

        #endregion

        #endregion

        public ProductVM()
        {
            #region Init Commands

            RefreshData = new BaseCommand(ExecuteRefreshData);
            SearchData = new BaseCommand(ExecuteSearchData);
            FromCategoryData = new BaseCommand(ExecuteFromCategoryData);
            AddProduct = new BaseCommand(ExecuteAddProduct, CanExecuteModifyData);
            EditProduct = new BaseCommand(ExecuteEditProduct, CanExecuteModifyData);
            DeleteProduct = new BaseCommand(ExecuteDeleteProduct, CanExecuteModifyData);
            ModifyUnit = new BaseCommand(ExecuteModifyUnit, CanExecuteModifyData);
            ModifyCategory = new BaseCommand(ExecuteModifyCategory, CanExecuteModifyData);

            #endregion

            RefreshData.Execute(null);
        }
    }
}
