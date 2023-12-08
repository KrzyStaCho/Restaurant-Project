using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.Form;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowForm
{
    class WindowRecipeCategoryVM : BaseWindowVM
    {
        #region Private Fields

        private RestaurantEntity database;

        #endregion
        #region Public Fields

        public FormStateEnum FormState;

        #endregion
        #region Data Binding

        public RecipeCategoryForm FormModel { get; private set; }

        #endregion
        #region Commands

        public ICommand ConfirmData { get; }
        public ICommand DeleteData { get; }

        private void ExecuteConfirmData(object? parameter)
        {
            FormState = (FormModel.SelectedItem?.FirstItem == -1) ? FormStateEnum.CREATE : FormStateEnum.EDIT;
            CloseCommand.Execute(null);
        }
        private void ExecuteDeleteData(object? parameter)
        {
            if (FormModel.SelectedItem?.FirstItem == -1)
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
            return FormModel.IsDataCorrect();
        }
        private bool CanExecuteDeleteData(object? paramter)
        {
            return (FormModel.SelectedItem != null);
        }

        #endregion
        #region Functions

        private List<ObjectPair<int, string>> GetCategories()
        {
            List<ObjectPair<int, string>> categoryList = DBModelConstructor.GetRecipeCategoryNames(database);
            categoryList.Add(new ObjectPair<int, string>(-1, "+ New category"));
            return categoryList;
        }
        private void LoadCategory(int categoryId)
        {
            if (categoryId == -1) return;

            RecipeCategory? category = database.RecipeCategories.Find(categoryId);
            if (category == null) return;

            FormModel.FillInput(category);
        }

        #endregion

        public WindowRecipeCategoryVM(Window window)
            : base(window, "Modify recipe categories")
        {
            database = MainWindowVM.DatabaseInstance;
            FormState = FormStateEnum.CANCEL;

            #region Init Commands

            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);
            DeleteData = new BaseCommand(ExecuteDeleteData, CanExecuteDeleteData);

            #endregion

            FormModel = new RecipeCategoryForm(GetCategories(), LoadCategory);
        }
    }
}
