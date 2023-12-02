using FontAwesome5;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.ViewModel
{
    class DashboardVM : BaseChildViewModel
    {
        #region Private Fields
        #endregion
        #region Public Fields
        #endregion
        #region Data Binding

        public DashItem UserItem { get; private set; }
        public DashItem StorageStatsItem {  get; private set; }

        #endregion
        #region Commands
        #endregion
        #region Functions

        public void LoadData()
        {
            string content = string.Empty;
            #region UserInfo

            content = "Username: " + mainVM.ActiveUsername + Environment.NewLine;
            content += "Group: " + mainVM.ActiveUser.GroupName + Environment.NewLine;
            content += "Is admin: " + mainVM.ActiveUser.IsAdmin + Environment.NewLine;
            UserItem = new DashItem(EFontAwesomeIcon.Regular_User, "User info", content);
            OnPropertyChanged(nameof(UserItem));

            #endregion
            #region Storage Stats

            content = "Products count: " + database.Products.Count().ToString() + Environment.NewLine;
            StorageStatsItem = new DashItem(EFontAwesomeIcon.Solid_Boxes, "Storage stats", content);

            #endregion
        }

        #endregion

        public DashboardVM() { }
    }
}
