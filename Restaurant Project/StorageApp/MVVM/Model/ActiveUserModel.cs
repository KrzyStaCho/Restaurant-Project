using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model
{
    public class ActiveUserModel
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string GroupName { get; set; }
        public bool IsAdmin { get; set; }
        public DateTime? LastOnline { get; set; }

        private List<string> permList;
        public Account RawUserProfile { get; private set; }

        public bool HasPerm(string permName)
        {
            if (permList == null) return false;
            if (IsAdmin) return true;

            string? targetPerm = permList.FirstOrDefault(p => p.ToLower().Equals(permName.ToLower()));
            if (targetPerm == null) return false;
            return true;
        }

        private ActiveUserModel() { }

        public static ActiveUserModel GetUserProfile(Account account, List<string> perms)
        {
            ActiveUserModel profile = new ActiveUserModel();
            profile.Id = account.AccountId;
            profile.Username = account.Username;
            profile.GroupName = (account.Group == null) ? string.Empty : account.Group.GroupName;
            profile.IsAdmin = (account.Group == null) ? false : account.Group.IsAdmin;
            profile.LastOnline = account.LastOnline;
            profile.permList = perms;
            profile.RawUserProfile = account;

            return profile;
        }
    }
}
