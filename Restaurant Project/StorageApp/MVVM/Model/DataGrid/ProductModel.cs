using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.DataGrid
{
    public class ProductModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal InStock { get; set; }
        public string UnitCode { get; set; }
        public string CategoryName { get; set; }
        public string SupplierCompanyName { get; set; }

        public ProductModel() { }
    }
}
