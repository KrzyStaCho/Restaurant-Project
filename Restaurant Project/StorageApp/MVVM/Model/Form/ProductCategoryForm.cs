using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.Form
{
    public class ProductCategoryForm : BaseProperty
    {
        #region Fields

        #region Static Fields

        public static readonly string EmptyBoxesError = "Category Name cannot be empty!";
        public static readonly string NotEnoughtLetterStandartError = "must have more than 2 character";

        #endregion

        public Action<ProductCategoryModel?> LoadFunction;
        public ProductCategory? ActionTarget { get; private set; }

        #region Data Binding

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private List<ProductCategoryModel> categories;
        public List<ProductCategoryModel> CategoryList
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(CategoryList));
            }
        }

        private ProductCategoryModel? _selected;
        public ProductCategoryModel? SelectedCategory
        {
            get { return _selected; }
            set
            {
                _selected = value;
                IsEnabled = true;
                Clear();
                if (value != null && value.ID != -1) LoadFunction.Invoke(value);
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private string _categoryName;
        public string CategoryName
        {
            get { return _categoryName; }
            set
            {
                _categoryName = value;
                OnPropertyChanged(nameof(CategoryName));
            }
        }

        private string _description;
        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        #endregion

        #endregion
        #region Functions

        public void Clear()
        {
            CategoryName = string.Empty;
            Description = string.Empty;
            Error = string.Empty;
        }
        public void TrimData()
        {
            CategoryName = CategoryName.Trim();
            Description = Description.Trim();
        }
        public void FillData(ProductCategory model)
        {
            CategoryName = model.CategoryName;
            Description = (model.Description == null) ? string.Empty : model.Description;
            ActionTarget = model;
        }
        public bool IsDataEmpty()
        {
            if (string.IsNullOrWhiteSpace(CategoryName)) return true;
            return false;
        }
        public bool CheckData()
        {
            //Check if urgent data is not empty
            if (IsDataEmpty()) { Error = EmptyBoxesError; return false; }
            TrimData();

            //Check if Category Name have more character than 2
            if (CategoryName.Length < 3) { Error = NotEnoughtLetterStandartError; return false; }

            Error = string.Empty;
            return true;
        }

        public ProductCategory GetFilledCategory()
        {
            ProductCategory? targetCategory = ActionTarget;
            if (targetCategory == null) targetCategory = new ProductCategory();

            //Fill data
            targetCategory.CategoryName = CategoryName;
            targetCategory.Description = (string.IsNullOrWhiteSpace(Description)) ? null : Description;

            return targetCategory;
        }

        #endregion

        public ProductCategoryForm(Action<ProductCategoryModel> loadFunction)
        {
            LoadFunction = loadFunction;
            ActionTarget = null;
            Clear();
        }
    }
}
