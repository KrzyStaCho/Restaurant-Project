using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.DataGrid
{
    class SupplierModel
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNIP { get; set; }
        public string ContactTitle { get; set; }
        public string Phone { get; set; }

        public SupplierModel(Supplier supplier)
        {
            SupplierID = supplier.SupplierId;
            CompanyName = supplier.CompanyName;
            CompanyNIP = supplier.CompanyNip;
            ContactTitle = supplier.ContactTitle ?? string.Empty;
            Phone = supplier.Phone ?? string.Empty;
        }
    }
}
