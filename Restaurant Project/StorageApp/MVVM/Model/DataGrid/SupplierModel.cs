using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.DataGrid
{
    public class SupplierModel
    {
        public int SupplierID { get; set; }
        public string CompanyName { get; set; }
        public string CompanyNIP { get; set; }
        public string ContactTitle { get; set; }
        public string Phone { get; set; }
        public SupplierModel() { }
    }
}
