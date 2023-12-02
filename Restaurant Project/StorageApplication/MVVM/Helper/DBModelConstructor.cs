using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ProjectLibrary.Repository.Context;
using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StorageApplication.MVVM.Model.Item;

namespace StorageApplication.MVVM.Helper
{
    static class DBModelConstructor
    {
        #region Account

        public static Account? GetAccount(RestaurantEntity database, string username, string hashedPassword)
        {
            return database.Accounts.Include(a => a.Group).
                ThenInclude(ag => ag.Permissions).
                FirstOrDefault(a => (a.Username.Equals(username) && a.Password.Value.Equals(hashedPassword)));
        }

        #endregion
        #region Supplier

        public static List<Supplier> GetSuppliers(RestaurantEntity database, string searching = "")
        {
            List<Supplier>? suppliers = database.Suppliers.Where(sp => sp.CompanyName.Contains(searching)).Take(100).ToList();
            return (suppliers ?? new List<Supplier>());
        }
        public static void RemoveSupplier(RestaurantEntity database, Supplier supplier)
        {
            //Remove supplier
            EntityEntry<Supplier> entry = database.Suppliers.Entry(supplier);
            entry.State = EntityState.Deleted;

            //Insert into archive table deleted supplier
            database.SupplierArchives.Add(new SupplierArchive()
            {
                CompanyName = supplier.CompanyName,
                CompanyNip = supplier.CompanyName,
                Address = supplier.Address,
                City = supplier.City,
                ContactName = supplier.ContactName,
                ContactTitle = supplier.ContactTitle,
                Phone = supplier.Phone,
                HomePage = supplier.HomePage,
                WhoChanged = supplier.WhoChanged,
                LastModified = supplier.LastModified,
                SupplierId = supplier.SupplierId
            });

            database.SaveChanges();
        }

        #endregion
        #region Product

        public static List<Product> GetProducts(RestaurantEntity database, string searching = "")
        {
            List<Product>? products = database.Products.Where(p => p.ProductName.Contains(searching)).Include(p => p.Supplier).
                Include(p => p.Category).Include(p => p.Unit).Take(100).ToList();
            return (products ?? new List<Product>());
        }
        public static Product? GetProduct(RestaurantEntity database, int productId)
        {
            return database.Products.Include(p => p.Category).Include(p => p.Supplier).Include(p => p.Unit).FirstOrDefault(p => p.ProductId == productId);
        }
        public static List<Product> GetProductsByCategory(RestaurantEntity database, int categoryId)
        {
            List<Product>? products = database.Products.Where(p => p.CategoryId == categoryId).Include(p => p.Supplier).
                Include(p => p.Category).Include(p => p.Unit).Take(100).ToList();
            return (products ?? new List<Product>());
        }
        public static void RemoveProduct(RestaurantEntity database, Product product)
        {
            //Remove supplier
            EntityEntry<Product> entry = database.Products.Entry(product);
            entry.State = EntityState.Deleted;

            //Insert into archive table deleted product
            database.ProductArchives.Add(new ProductArchive()
            {
                ProductName = product.ProductName,
                UnitCode = product.Unit.Code,
                CategoryName = product.Category?.CategoryName ?? null,
                SupplierName = product.Supplier?.CompanyName ?? null,
                WhoChanged = product.WhoChanged,
                LastModified = product.LastModified,
                ProductId = product.ProductId
            });

            database.SaveChanges();
        }


        public static List<ProductCategory> GetProductCategories(RestaurantEntity database, string searching = "")
        {
            return database.ProductCategories.Where(pc => pc.CategoryName.Contains(searching)).ToList();
        }
        public static List<ObjectPair<int, string>> GetCategoryName(RestaurantEntity database)
        {
            List<ObjectPair<int, string>> categoryNames = database.ProductCategories.Select(pc => new ObjectPair<int, string>(pc.CategoryId, pc.CategoryName)).ToList();
            return (categoryNames ?? new List<ObjectPair<int, string>>());
        }
        public static void RemoveCategory(RestaurantEntity database, ProductCategory category)
        {
            EntityEntry<ProductCategory> entry = database.ProductCategories.Entry(category);
            entry.State = EntityState.Deleted;

            database.SaveChanges();
        }

        public static List<MeasureUnit> GetMeasureUnits(RestaurantEntity database)
        {
            return database.MeasureUnits.ToList();
        }
        public static void RemoveMeasureUnit(RestaurantEntity database, MeasureUnit measureUnit)
        {
            EntityEntry<MeasureUnit> entry = database.MeasureUnits.Entry(measureUnit);
            entry.State = EntityState.Deleted;

            database.SaveChanges();
        }

        #endregion
    }
}