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
    class WindowRecipeVM : BaseWindowVM
    {
        #region Private Fields

        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public FormStateEnum FormState;

        #endregion
        #region Data Binding

        public RecipeForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand GetCategory { get; }
        public ICommand AddProduct { get; }
        public ICommand DeleteProduct { get; }
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
        private void ExecuteAddProduct(object? parameter)
        {
            Window productWindow = new WindowProductSelect();
            productWindow.Owner = mainWindow;
            BaseWindowSelectVM<Product> productVM = new BaseWindowSelectVM<Product>(productWindow, "Choose product", LoadProducts);

            productWindow.ShowDialog();

            //Check if user confirm
            Product? model = productVM.SelectedObject;
            if (model == null) return;

            Window quantityWindow = new WindowValTextSelect();
            quantityWindow.Owner = mainWindow;
            BaseTextWindowSelectVM quantityVM = new BaseTextWindowSelectVM(quantityWindow, "Insert quantity", "Quantity: ", ValidationClass.CheckDecimal);

            quantityWindow.ShowDialog();

            //Check if user confirm
            if (quantityVM.FormState != FormStateEnum.CONFIRM) return;

            decimal quantity = Convert.ToDecimal(quantityVM.Value);

            //Add product
            FormModel.AddProductToList(new Model.DataGrid.RecipeDetailModel(model, quantity));
        }
        private void ExecuteDeleteProduct(object? parameter)
        {
            //Validate parameter
            if (parameter is not RecipeDetailModel) return;
            RecipeDetailModel model = (RecipeDetailModel)parameter;

            FormModel.RemoveProductFromList(model);
        }
        private void ExecuteConfirmData(object? parameter)
        {
            FormState = FormStateEnum.CONFIRM;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            return FormModel.IsDataCorrect();
        }

        #endregion
        #region Functions

        private List<ObjectPair<int, string>> LoadCategory(string searching)
        {
            List<RecipeCategory> rawCategory = DBModelConstructor.GetRecipeCategories(database, searching);

            return rawCategory.Select(rc => new ObjectPair<int, string>(rc.CategoryId, rc.CategoryName)).ToList();
        }
        private List<Product> LoadProducts(string searching)
        {
            return DBModelConstructor.GetProducts(database, searching);
        }

        #endregion

        public WindowRecipeVM(Window window, Recipe? model = null)
            : base(window, model == null ? "Add recipe" : "Edit recipe")
        {
            database = MainWindowVM.DatabaseInstance;
            FormState = FormStateEnum.CANCEL;

            #region Init Commands

            GetCategory = new BaseCommand(ExecuteGetCategory);
            AddProduct = new BaseCommand(ExecuteAddProduct);
            DeleteProduct = new BaseCommand(ExecuteDeleteProduct);
            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);

            #endregion

            FormModel = new RecipeForm(GetCategory, AddProduct, DeleteProduct);
            if (model != null)
            {
                FormModel.FillInput(model);
            }
        }
    }
}
