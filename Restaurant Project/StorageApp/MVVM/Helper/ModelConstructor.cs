using Microsoft.EntityFrameworkCore;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.DataGrid;
using StorageApp.MVVM.Model.Menu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Helper
{
    public static class ModelConstructor
    {
        #region Help Function

        private static readonly Func<Supplier, SupplierModel> ParseRawSupplier = (Supplier s) =>
        {
            return new SupplierModel()
            {
                SupplierID = s.SupplierId,
                CompanyName = s.CompanyName,
                CompanyNIP = s.CompanyNip,
                ContactTitle = (s.ContactTitle == null) ? string.Empty : s.ContactTitle,
                Phone = (s.Phone == null) ? string.Empty : s.Phone
            };
        };
        private static readonly Func<Product, ProductModel> ParseRawProduct = (Product s) =>
        {
            return new ProductModel()
            {
                ProductID = s.ProductId,
                ProductName = s.ProductName,
                InStock = s.InStock,
                UnitCode = s.Unit.Code,
                CategoryName = (s.Category == null) ? string.Empty : s.Category.CategoryName,
                SupplierCompanyName = (s.Supplier == null) ? string.Empty : s.Supplier.CompanyName
            };
        };

        #endregion

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
                suppliers = database.Suppliers.Take(100).Select(ParseRawSupplier).ToList();
            }
            else
            {
                suppliers = database.Suppliers.Where(s => s.CompanyName.Contains(searching)).Take(100).Select(ParseRawSupplier).ToList();
            }
            return (suppliers == null) ? new List<SupplierModel>() : suppliers;
        }

        #endregion
        #region Product

        public static List<CategoryMenu> GetCategoryMenu(RestaurantEntity database)
        {
            List<CategoryMenu>? categoryMenu = database.ProductCategories.Select(pc => new CategoryMenu(pc.CategoryName, pc.CategoryId)).ToList();
            return (categoryMenu == null) ? new List<CategoryMenu>() : categoryMenu;
        }
        public static List<ProductModel> GetProducts(RestaurantEntity database, string searching)
        {
            List<ProductModel>? products = null;
            if (string.IsNullOrWhiteSpace(searching))
            {
                products = database.Products.Take(100).Include(p => p.Unit).Include(p => p.Category).Include(p => p.Supplier).Select(ParseRawProduct).ToList();
            }
            else
            {
                products = database.Products.Where(p => p.ProductName.Contains(searching)).Take(100).Include(p => p.Unit).Include(p => p.Category)
                    .Include(p => p.Supplier).Select(ParseRawProduct).ToList();
            }
            return (products == null) ? new List<ProductModel>() : products;
        }
        public static List<ProductModel> SearchProductsByCategory(RestaurantEntity database, int categoryID)
        {
            List<ProductModel>? products = database.Products.Include(p => p.Category).Where(p => (p.CategoryId == null) ? false : (p.CategoryId == categoryID)).Take(100).Include(p => p.Unit)
                .Include(p => p.Supplier).Select(ParseRawProduct).ToList();
            return (products == null) ? new List<ProductModel>() : products;
        }
        public static List<ProductCategoryModel> GetProductCategories(RestaurantEntity database)
        {
            List<ProductCategoryModel>? categories = database.ProductCategories.Select(pc => new ProductCategoryModel()
            {
                ID = pc.CategoryId,
                Name = pc.CategoryName,
                Description = (pc.Description == null) ? string.Empty : pc.Description
            }).ToList();
            return (categories == null) ? new List<ProductCategoryModel>() : categories;
        }
        public static ProductCategory GetProductCategory(RestaurantEntity database, int categoryID)
        {
            ProductCategory? category = database.ProductCategories.FirstOrDefault(pc => pc.CategoryId == categoryID);
            return (category == null) ? new ProductCategory() : category;
        }

        #endregion
    }
}
