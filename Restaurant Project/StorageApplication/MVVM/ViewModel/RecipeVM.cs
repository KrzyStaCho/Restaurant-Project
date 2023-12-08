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
    class RecipeVM : BaseChildViewModel
    {
        #region Private Fields

        private List<ObjectPair<int, string>> _categoryMenu;
        private List<RecipeModel> _recipeList;
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
        public List<RecipeModel> RecipeList
        {
            get { return _recipeList; }
            set
            {
                _recipeList = value;
                OnPropertyChanged(nameof(RecipeList));
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
        public ICommand AddRecipe { get; }
        public ICommand EditRecipe { get; }
        public ICommand DeleteRecipe { get; }
        public ICommand ModifyCategories { get; }

        private void ExecuteRefreshData(object? parameter)
        {
            SearchBox = string.Empty;
            LoadFullData();
        }
        private void ExecuteSearchData(object? parameter)
        {
            LoadRecipes(SearchBox);
        }
        private void ExecuteFromCategoryData(object? parameter)
        {
            if (parameter is not int) return;
            int categoryId = (int)parameter;

            LoadRecipesByCategory(categoryId);
        }
        private void ExecuteAddRecipe(object? parameter)
        {
            Window addWindow = mainVM.SetOwner(new WindowRecipeForm());
            WindowRecipeVM addRecipeVM = new WindowRecipeVM(addWindow);

            addWindow.ShowDialog();

            //Check if user confirm changes
            if (addRecipeVM.FormState != Model.Form.FormStateEnum.CONFIRM) return;

            //Add user info to model
            Recipe toAddRecipe = addRecipeVM.FormModel.GetInput();
            toAddRecipe.WhoChanged = mainVM.ActiveUsername;
            toAddRecipe.LastModified = DateTime.Now.Date;

            //Add recipe to DB
            database.Recipes.Add(toAddRecipe);
            database.SaveChanges();

            //Add recipe detail to DB
            List<RecipeDetail> details = addRecipeVM.FormModel.GetDetails().ToList();
            details.ForEach(d => d.RecipeId = toAddRecipe.RecipeId);
            database.RecipeDetails.AddRange(details);
            database.SaveChanges();

            mainVM.IncrementChangesInDB();

            //Refresh view
            RefreshData.Execute(null);
        }
        private void ExecuteEditRecipe(object? parameter)
        {
            //Validate parameter
            if (parameter is not RecipeModel) return;
            RecipeModel model = (RecipeModel)parameter;

            //Get raw model
            Recipe? rawModel = DBModelConstructor.GetRecipe(database, model.RecipeID);
            if (rawModel == null) return;

            Window editWindow = mainVM.SetOwner(new WindowRecipeForm());
            WindowRecipeVM editRecipeVM = new WindowRecipeVM(editWindow, rawModel);

            editWindow.ShowDialog();

            //Check if user confirm changes
            if (editRecipeVM.FormState != Model.Form.FormStateEnum.CONFIRM) return;

            //Add user info to model
            Recipe toEditRecipe = editRecipeVM.FormModel.GetInput();
            toEditRecipe.WhoChanged = mainVM.ActiveUsername;
            toEditRecipe.LastModified = DateTime.Now.Date;

            List<RecipeDetail> currentDetail = toEditRecipe.RecipeDetails.ToList();
            List<RecipeDetail> newDetail = editRecipeVM.FormModel.GetDetails();
            newDetail.ForEach(nd => nd.RecipeId = toEditRecipe.RecipeId);

            //Remove details from DB
            List<RecipeDetail> toRemoveDetail = currentDetail.Where(cd => !newDetail.Any(nd => nd.ProductId == cd.ProductId)).ToList();
            database.RecipeDetails.RemoveRange(toRemoveDetail);

            //Add details to DB
            List<RecipeDetail> toAddDetail = newDetail.Where(nd => !currentDetail.Any(cd => cd.ProductId == nd.ProductId)).ToList();
            toAddDetail.ForEach(ad => database.RecipeDetails.Add(ad));

            //Edit details in DB
            currentDetail.ForEach(cd =>
            {
                RecipeDetail? newRecipe = newDetail.FirstOrDefault(nd => nd.ProductId == cd.ProductId);
                if (newRecipe != null)
                {
                    cd.Quantity = newRecipe.Quantity;
                }
            });

            //Save changes in DB
            database.SaveChanges();
            mainVM.IncrementChangesInDB();

            //Refresh view
            RefreshData.Execute(null);
        }
        private void ExecuteDeleteRecipe(object? parameter)
        {
            //Validate parameter
            if (parameter is not RecipeModel) return;
            RecipeModel model = (RecipeModel)parameter;

            //Get raw model
            Recipe? toRemoveRecipe = DBModelConstructor.GetRecipe(database, model.RecipeID);
            if (toRemoveRecipe == null) return;

            //Get confirm from user
            MessageBoxResult result = MessageBox.Show("Are you sure about delete that recipe?", "Delete confirm action", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes) return;

            //Add user data to product
            toRemoveRecipe.WhoChanged = mainVM.ActiveUsername;
            toRemoveRecipe.LastModified = DateTime.Now.Date;

            //Remove from DB
            DBModelConstructor.RemoveRecipe(database, toRemoveRecipe);
            mainVM.IncrementChangesInDB();

            //Refresh data
            RefreshData.Execute(null);
        }
        private void ExecuteModifyCategories(object? parameter)
        {
            Window modifyWindow = mainVM.SetOwner(new WindowProductCategoryForm());
            WindowRecipeCategoryVM modifyCategoryVM = new WindowRecipeCategoryVM(modifyWindow);

            modifyWindow.ShowDialog();

            //Check if user confirm changes and do target action
            switch (modifyCategoryVM.FormState)
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
            RefreshData.Execute(null);
        }

        private bool CanExecuteModifyData(object? parameter)
        {
            if (mainVM.ActiveUser == null) return false;
            return mainVM.ActiveUser.HasPerm(ProgramCode.ModifyRecipeCode);
        }

        #endregion
        #region Functions

        private void LoadFullData()
        {
            LoadCategoryMenu();
            LoadRecipes(string.Empty);
        }
        private void LoadRecipes(string searching)
        {
            List<Recipe> recipes = DBModelConstructor.GetRecipe(database, searching);
            database.ChangeTracker.Clear();

            RecipeList = recipes.Select(r => new RecipeModel(r)).ToList();
        }
        private void LoadRecipesByCategory(int categoryId)
        {
            List<Recipe> recipes = DBModelConstructor.GetRecipeByCategory(database, categoryId);
            database.ChangeTracker.Clear();

            RecipeList = recipes.Select(r => new RecipeModel(r)).ToList();
        }
        private void LoadCategoryMenu()
        {
            CategoryList = DBModelConstructor.GetRecipeCategoryNames(database);
        }

        #region Modify Category Help Function

        private void AddCategory(RecipeCategory category)
        {
            database.RecipeCategories.Add(category);
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void EditCategory(RecipeCategory category)
        {
            database.SaveChanges();

            database.ChangeTracker.Clear();
            mainVM.IncrementChangesInDB();
        }
        private void DeleteCategory(RecipeCategory category)
        {
            DBModelConstructor.RemoveRecipeCategory(database, category);
            mainVM.IncrementChangesInDB();
        }

        #endregion

        #endregion

        public RecipeVM()
        {
            #region Init Commands

            RefreshData = new BaseCommand(ExecuteRefreshData);
            SearchData = new BaseCommand(ExecuteSearchData);
            FromCategoryData = new BaseCommand(ExecuteFromCategoryData);
            AddRecipe = new BaseCommand(ExecuteAddRecipe, CanExecuteModifyData);
            EditRecipe = new BaseCommand(ExecuteEditRecipe, CanExecuteModifyData);
            DeleteRecipe = new BaseCommand(ExecuteDeleteRecipe, CanExecuteModifyData);
            ModifyCategories = new BaseCommand(ExecuteModifyCategories, CanExecuteModifyData);

            #endregion

            RefreshData.Execute(null);
        }
    }
}
