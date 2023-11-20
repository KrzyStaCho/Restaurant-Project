using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.DataGrid;
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
        #region Supplier

        public static List<SupplierModel> GetSuppliers(RestaurantEntity database, string searching)
        {
            List<SupplierModel>? suppliers = null;
            if (string.IsNullOrWhiteSpace(searching))
            {
                suppliers = database.Suppliers.Take(100).Select(s => new SupplierModel()
                {
                    SupplierID = s.SupplierId,
                    CompanyName = s.CompanyName,
                    ContactTitle = (s.ContactTitle == null) ? string.Empty : s.ContactTitle,
                    Phone = (s.Phone == null) ? string.Empty : s.Phone
                }).ToList();
            }
            else
            {
                suppliers = database.Suppliers.Where(s => s.CompanyName.Contains(searching)).Take(100).Select(s => new SupplierModel()
                {
                    SupplierID = s.SupplierId,
                    CompanyName = s.CompanyName,
                    ContactTitle = (s.ContactTitle == null) ? string.Empty : s.ContactTitle,
                    Phone = (s.Phone == null) ? string.Empty : s.Phone
                }).ToList();
            }
            return (suppliers == null) ? new List<SupplierModel>() : suppliers;
        }

        #endregion
    }
}
