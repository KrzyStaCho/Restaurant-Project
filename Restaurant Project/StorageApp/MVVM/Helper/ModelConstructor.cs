using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Helper
{
    public static class ModelConstructor
    {
        #region Account

        public static ActiveUserModel? GetUserProfile(RestaurantEntity database, string username, string password)
        {
            Account? rawAccount = database.Accounts.Where(a => (a.Username.Equals(username) && a.Password.Value.Equals(password))).Include(a => a.Group).Include(a => a.Group.Permissions).FirstOrDefault();
            if (rawAccount == null) { return null; }

            return ActiveUserModel.GetUserProfile(rawAccount);
        }

        #endregion
    }
}
