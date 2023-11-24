using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Helper;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.Model.Menu;
using StorageApp.MVVM.View.WindowForm;
using StorageApp.MVVM.ViewModel.Core;
using StorageApp.MVVM.ViewModel.WindowForm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel
{
    public class ProductVM : BaseViewModel
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        private List<CategoryMenu> _categoryMenu;
        private List<ProductModel> _product;
        private string searchbox;

        #endregion
        #region Public Fields

        #endregion
        #region Data Binding

        public List<CategoryMenu> CategoryList
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
            get { return _product; }
            set
            {
                _product = value;
                OnPropertyChanged(nameof(ProductList));
            }
        }
        public string Searchbox
        {
            get { return searchbox; }
            set
            {
                searchbox = value;
                OnPropertyChanged(nameof(Searchbox));
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
            LoadFullData();
        }
        private void ExecuteSearchData(object? parameter)
        {
            LoadProducts(Searchbox);
            Searchbox = string.Empty;
        }
        private void ExecuteFromCategoryData(object? parameter)
        {
            int? categoryID = (int?)parameter;
            if (parameter == null) return;

            ProductList = ModelConstructor.SearchProductsByCategory(database, (int)categoryID);
        }
        private void ExecuteModifyCategory(object? parameter)
        {
            Window modifyWindow = new WindowProductCategoryForm();
            Window owner = mainVM.GetWindow();
            WindowProductCategoryVM modifyCategoryVM = new WindowProductCategoryVM(modifyWindow, owner);

            modifyWindow.ShowDialog();

            //Check if user confirm changes and do target action
            switch(modifyCategoryVM.FormState)
            {
                case Model.Form.FormStateEnum.CREATE:
                    AddCategory(modifyCategoryVM.FormModel.GetFilledCategory());
                    LoadCategoryMenu();
                    break;
                case Model.Form.FormStateEnum.EDIT:
                    EditCategory(modifyCategoryVM.FormModel.GetFilledCategory());
                    LoadCategoryMenu();
                    break;
                case Model.Form.FormStateEnum.DELETE:
                    DeleteCategory(modifyCategoryVM.FormModel.GetFilledCategory());
                    LoadCategoryMenu();
                    break;
                case Model.Form.FormStateEnum.CANCEL:
                default:
                    return;
            }
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
            Searchbox = string.Empty;
            LoadCategoryMenu();
            LoadProducts(string.Empty);
        }
        private void LoadProducts(string searchBox)
        {
            ProductList = ModelConstructor.GetProducts(database, searchBox);
        }
        private void LoadCategoryMenu()
        {
            CategoryList = ModelConstructor.GetCategoryMenu(database);
        }

        #region Modify Category Help Function

        private void AddCategory(ProductCategory category)
        {
            category.CategoryId = 0;
            database.ProductCategories.Add(category);
            mainVM.IncrementChangesInDB();
            database.SaveChanges();
        }
        private void EditCategory(ProductCategory category)
        {
            mainVM.IncrementChangesInDB();
            database.SaveChanges();
        }
        private void DeleteCategory(ProductCategory category)
        {
            EntityEntry<ProductCategory> entityEntry = database.ProductCategories.Attach(category);
            entityEntry.State = EntityState.Deleted;

            mainVM.IncrementChangesInDB();
            database.SaveChanges();
        }

        #endregion

        #endregion

        public ProductVM()
        {
            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;

            #region Init Commands

            RefreshData = new BaseCommand(ExecuteRefreshData);
            SearchData = new BaseCommand(ExecuteSearchData);
            FromCategoryData = new BaseCommand(ExecuteFromCategoryData);

            ModifyCategory = new BaseCommand(ExecuteModifyCategory, CanExecuteModifyData);

            #endregion

            RefreshData.Execute(null);
        }
    }
}
