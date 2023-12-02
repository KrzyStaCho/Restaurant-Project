using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model
{
    class ActiveUserModel
    {
        #region Fields

        public int Id { get; private set; }
        public string Username { get; private set; }
        public string GroupName { get; private set; }
        public bool IsAdmin { get; private set; }
        public DateTime? LastOnline { get; private set; }

        private List<string> permList;

        #endregion
        #region Methods

        public bool HasPerm(string permName)
        {
            if (IsAdmin) return true;
            if (permList.Count == 0) return false;

            return (permList.FirstOrDefault(p => p.ToLower().Equals(permName.ToLower())) == null) ? false : true;
        }

        public static ActiveUserModel GetUserProfile(Account account)
        {
            return new ActiveUserModel()
            {
                Id = account.AccountId,
                Username = account.Username,
                GroupName = account.Group.GroupName,
                IsAdmin = account.Group.IsAdmin,
                LastOnline = account.LastOnline,
                permList = account.Group.Permissions.Select(p => p.Code).ToList()
            };
        }

        #endregion

        private ActiveUserModel() { }
    }
}
