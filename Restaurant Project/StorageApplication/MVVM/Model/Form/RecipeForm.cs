using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.DataGrid;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApplication.MVVM.Model.Form
{
    class RecipeForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        private bool isValid = false;

        public Recipe? ActionTarget { get; private set; }

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch(name)
                {
                    case nameof(RecipeName):
                        result = ValidationClass.CheckText(RecipeName, nameof(RecipeName));
                        isValid = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private string _recipeName = string.Empty;
        public string RecipeName
        {
            get { return _recipeName; }
            set
            {
                _recipeName = value.Trim();
                OnPropertyChanged(nameof(RecipeName));
            }
        }

        private ObjectPair<int, string> _selectedCategory;
        public ObjectPair<int, string> SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }

        private List<RecipeDetailModel> _products = new List<RecipeDetailModel>();
        public List<RecipeDetailModel> Products
        {
            get { return _products; }
            set
            {
                _products = value;
                OnPropertyChanged(nameof(Products));
                OnPropertyChanged(nameof(ProductCount));
            }
        }

        public string ProductCount
        {
            get { return "Products: " + Products.Count; }
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

        public ICommand GetCategory { get; }
        public ICommand AddProduct { get; }
        public ICommand DeleteProduct { get; }

        #endregion

        #endregion
        #region Methods

        public void AddProductToList(RecipeDetailModel model)
        {
            List<RecipeDetailModel> list = Products;
            list.Add(model);

            Products = new List<RecipeDetailModel>();
            Products = list;
        }
        public void RemoveProductFromList(RecipeDetailModel model)
        {
            List<RecipeDetailModel> list = Products;
            list.Remove(model);

            Products = new List<RecipeDetailModel>();
            Products = list;
        }
        public void ClearInput()
        {
            RecipeName = string.Empty;
            SelectedCategory = ValidationClass.EmptyFKey;
            Products = new List<RecipeDetailModel>();
            Error = string.Empty;
        }
        public void FillInput(Recipe model)
        {
            RecipeName = model.RecipeName;

            if (model.Category == null) SelectedCategory = ValidationClass.EmptyFKey;
            else SelectedCategory = new ObjectPair<int, string>(model.Category.CategoryId, model.Category.CategoryName);

            Products = model.RecipeDetails.Select(rd => new RecipeDetailModel(rd)).ToList();

            ActionTarget = model;
            Error = string.Empty;
        }
        public Recipe GetInput()
        {
            Recipe? filledRecipe = ActionTarget;
            if (filledRecipe == null) filledRecipe = new Recipe();

            filledRecipe.RecipeName = RecipeName;
            filledRecipe.CategoryId = (SelectedCategory.FirstItem == -1) ? null : SelectedCategory.FirstItem;

            return filledRecipe;
        }
        public List<RecipeDetail> GetDetails()
        {
            return Products.Select(p => new RecipeDetail()
            {
                ProductId = p.ProductID,
                Quantity = p.Quantity
            }).DistinctBy(d => d.ProductId).ToList();
        }
        public bool IsDataCorrect()
        {
            return isValid;
        }

        #endregion

        public RecipeForm(ICommand getCategory, ICommand addProduct, ICommand deleteProduct)
        {
            ActionTarget = null;
            SelectedCategory = ValidationClass.EmptyFKey;

            GetCategory = getCategory;
            AddProduct = addProduct;
            DeleteProduct = deleteProduct;
        }
    }
}
