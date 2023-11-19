using ProjectLibrary.Repository.Context;
using StorageApp.MVVM.Model.Item;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.ViewModel
{
    public class DashboardVM : BaseViewModel
    {
        #region Private Fields

        private MainWindowVM mainVM;
        private RestaurantEntity database;

        #endregion
        #region Public Fields

        #endregion
        #region Data Binding

        public DashItem UserItem { get; private set; }

        #endregion
        #region Commands

        #endregion
        #region Functions

        public void LoadData()
        {
            string content = string.Empty;
            #region UserInfo

            content = "Username: " + mainVM.ActiveUser.Username + Environment.NewLine;
            content += "Group: " + mainVM.ActiveUser.GroupName + Environment.NewLine;
            content += "Is admin: " + mainVM.ActiveUser.IsAdmin;
            UserItem = new DashItem("Regular_User", "User info", content);
            OnPropertyChanged(nameof(UserItem));

            #endregion
        }

        #endregion

        public DashboardVM()
        {
            mainVM = MainWindowVM.Instance;
            database = mainVM.DatabaseInstance;
        }
    }
}
