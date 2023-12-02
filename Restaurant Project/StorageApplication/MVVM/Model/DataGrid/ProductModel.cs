using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.DataGrid
{
    class ProductModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal InStock { get; set; }
        public string UnitCode { get; set; }
        public string CategoryName { get; set; }
        public string SupplierCompanyName { get; set; }

        public ProductModel(Product model)
        {
            ProductID = model.ProductId;
            ProductName = model.ProductName;
            InStock = model.InStock;
            UnitCode = model.Unit.Code;
            CategoryName = model.Category?.CategoryName ?? string.Empty;
            SupplierCompanyName = model.Supplier?.CompanyName ?? string.Empty;
        }
    }
}
