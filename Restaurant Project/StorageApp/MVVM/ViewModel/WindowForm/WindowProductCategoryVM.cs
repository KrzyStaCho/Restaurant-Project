using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Helper;
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
    public class WindowProductCategoryVM : BaseWindowVM
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public FormStateEnum FormState;

        #endregion
        #region Data Binding

        public ProductCategoryForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand ConfirmData { get; }
        public ICommand DeleteData { get; }

        private void ExecuteConfirmData(object? parameter)
        {
            if (FormModel.SelectedCategory == null) return;
            if (!FormModel.CheckData()) return;

            FormState = (FormModel.SelectedCategory.ID == -1) ? FormStateEnum.CREATE : FormStateEnum.EDIT;
            CloseCommand.Execute(null);
        }
        private void ExecuteDeleteData(object? parameter)
        {
            if (FormModel.SelectedCategory == null) return;

            if (FormModel.SelectedCategory.ID == -1)
            {
                FormModel.Error = "Cannot delete category that hasn't created yet!";
                return;
            }

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that category?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            FormState = FormStateEnum.DELETE;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            if (FormModel.SelectedCategory == null) return false;
            return !(FormModel.IsDataEmpty());
        }
        private bool CanExecuteDeleteData(object? parameter)
        {
            if (FormModel.SelectedCategory == null) return false;
            return true;
        }

        #endregion
        #region Functions

        private void LoadCategories()
        {
            List<ProductCategoryModel> categoryList = ModelConstructor.GetProductCategories(database);
            categoryList.Add(ProductCategoryModel.GetEmptyModel());
            FormModel.CategoryList = categoryList;
            FormModel.IsEnabled = false;
        }
        private void LoadCategory(ProductCategoryModel? model)
        {
            if (model == null) return;
            FormModel.FillData(ModelConstructor.GetProductCategory(database, model.ID));
        }

        #endregion

        public WindowProductCategoryVM(Window window, Window owner)
            : base(window, "Modify Product Category")
        {
            window.Owner = owner;

            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;

            FormState = FormStateEnum.CANCEL;

            #region Init Commands

            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);
            DeleteData = new BaseCommand(ExecuteDeleteData, CanExecuteDeleteData);

            #endregion

            FormModel = new ProductCategoryForm(LoadCategory);
            LoadCategories();
        }
    }
}
