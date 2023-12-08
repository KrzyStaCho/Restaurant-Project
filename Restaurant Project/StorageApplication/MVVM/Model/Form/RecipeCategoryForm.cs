using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.Form
{
    class RecipeCategoryForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        private bool[] isValid = new bool[2];

        private Action<int> LoadFunction;
        public RecipeCategory? ActionTarget { get; private set; }

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch (name)
                {
                    case nameof(CategoryName):
                        result = ValidationClass.CheckText(CategoryName, nameof(CategoryName));
                        isValid[0] = (result == null);
                        break;
                    case nameof(Description):
                        result = ValidationClass.CheckOptionalText(Description, nameof(Description));
                        isValid[1] = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private bool _isEnabled = false;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private List<ObjectPair<int, string>> categories;
        public List<ObjectPair<int, string>> CategoryList
        {
            get { return categories; }
            set
            {
                categories = value;
                OnPropertyChanged(nameof(CategoryList));
            }
        }
        private ObjectPair<int, string>? selectedItem;
        public ObjectPair<int, string>? SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                IsEnabled = (value != null);
                ClearInput();
                if (value != null && value.FirstItem != -1) LoadFunction.Invoke(value.FirstItem);
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string categoryName = string.Empty;
        public string CategoryName
        {
            get { return categoryName; }
            set
            {
                categoryName = value.Trim();
                OnPropertyChanged(nameof(CategoryName));
            }
        }
        
        private string description = string.Empty;
        public string Description
        {
            get { return description; }
            set
            {
                description = value.Trim();
                OnPropertyChanged(nameof(Description));
            }
        }

        private string _error = string.Empty;
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
        #region Methods

        public void ClearInput()
        {
            CategoryName = string.Empty;
            Description = string.Empty;

            Error = string.Empty;
        }
        public void FillInput(RecipeCategory model)
        {
            CategoryName = model.CategoryName;
            Description = model.Description ?? string.Empty;

            Error = string.Empty;
            ActionTarget = model;
        }
        public RecipeCategory GetInput()
        {
            RecipeCategory? filledCategory = ActionTarget;
            if (filledCategory == null) filledCategory = new RecipeCategory();

            filledCategory.CategoryName = CategoryName;
            filledCategory.Description = (ValidationClass.IsEmpty(Description)) ? null : Description;

            return filledCategory;
        }
        public bool IsDataCorrect()
        {
            return isValid.All(v => v);
        }

        #endregion

        public RecipeCategoryForm(List<ObjectPair<int, string>> categoryList, Action<int> loadFunction)
        {
            LoadFunction = loadFunction;
            CategoryList = categoryList;
            ActionTarget = null;
        }
    }
}
