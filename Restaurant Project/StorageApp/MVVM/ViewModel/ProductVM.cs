using ProjectLibrary.Repository.Context;
using StorageApp.MVVM.Helper;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.Model.Menu;
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
                OnPropertyChanged(nameof(CategoryMenu));
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

        #endregion

        public ProductVM()
        {
            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;

            #region Init Commands

            RefreshData = new BaseCommand(ExecuteRefreshData);
            SearchData = new BaseCommand(ExecuteSearchData);
            FromCategoryData = new BaseCommand(ExecuteFromCategoryData);

            #endregion

            RefreshData.Execute(null);
        }
    }
}
