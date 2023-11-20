using ProjectLibrary.Repository.Context;
using StorageApp.MVVM.Helper;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.Form;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel
{
    public class LogInVM : BaseViewModel
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        #endregion
        #region Data Binding

        public LogInForm FormModel { get; set; }

        #endregion
        #region Commands

        public ICommand LogIn { get; }

        private void ExecuteLogIn(object? parameter)
        {
            if (!FormModel.CheckData()) return;
            FormModel.TrimData();

            //Check if data is valid and get UserProfile if true
            ActiveUserModel? userProfile = ModelConstructor.GetUserProfile(database, FormModel.Username, FormModel.GetHashedPassword());
            if (userProfile == null) { FormModel.Error = LogInForm.IncorrectDataError; return; }

            //Check if user can log in to application
            if (!userProfile.HasPerm(ProgramCode.LogInPermissionCode)) { FormModel.Error = LogInForm.IncorrectDataError; return; }

            //Update LastOnline properties
            userProfile.RawUserProfile.LastOnline = DateTime.Now.Date;
            database.SaveChanges();

            //Save profile
            mainVM.ActiveUser = userProfile;

            //Clear data
            FormModel.Clear();

            //Switch view
            mainVM.SwitchToDashboardView.Execute(null);
        }
        private bool CanExecuteLogIn(object? parameter)
        {
            return (!FormModel.IsDataEmpty());
        }

        #endregion

        public LogInVM()
        {
            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;

            #region Init Commands

            LogIn = new BaseCommand(ExecuteLogIn, CanExecuteLogIn);

            #endregion

            FormModel = new LogInForm(LogIn);
        }
    }
}
