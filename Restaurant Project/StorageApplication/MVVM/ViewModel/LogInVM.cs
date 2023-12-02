using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model;
using StorageApplication.MVVM.Model.Form;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel
{
    class LogInVM : BaseChildViewModel
    {
        #region Private Fields
        #endregion
        #region Public Fields

        #endregion
        #region Data Binding

        public LogInForm FormModel { get; set; }

        #endregion
        #region Commands

        public ICommand LogIn { get; }

        private void ExecuteLogIn(object? parameter)
        {
            if (!FormModel.IsDataCorrect()) return;

            //Check if data is valid  and get UserProfile if true
            Account? targetAccount = DBModelConstructor.GetAccount(database, FormModel.Username, FormModel.GetHashedPassword());
            if (targetAccount == null) { FormModel.Error = LogInForm.IncorrectDataError; return; }
            ActiveUserModel userProfile = ActiveUserModel.GetUserProfile(targetAccount);

            //Check if user can log in to application
            if (!userProfile.HasPerm(ProgramCode.LogInPermissionCode)) { FormModel.Error = LogInForm.IncorrectDataError; return; }

            //Update LastOnline properties
            targetAccount.LastOnline = DateTime.Now.Date;
            database.SaveChanges();
            database.ChangeTracker.Clear();

            //Save profile
            mainVM.ActiveUser = userProfile;

            //Clear data
            FormModel.ClearInput();

            //Switch View
            mainVM.SwitchToDashboardView.Execute(null);
        }

        private bool CanExecuteLogIn(object? parameter)
        {
            return FormModel.IsDataCorrect();
        }

        #endregion
        #region Functions
        #endregion

        public LogInVM()
        {
            #region Init Commands

            LogIn = new BaseCommand(ExecuteLogIn, CanExecuteLogIn);

            #endregion

            FormModel = new LogInForm(LogIn);
        }
    }
}
