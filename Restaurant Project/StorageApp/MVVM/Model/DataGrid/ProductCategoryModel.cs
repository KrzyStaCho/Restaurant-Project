using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.DataGrid
{
    public class ProductCategoryModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public ProductCategoryModel() { }

        public static ProductCategoryModel GetEmptyModel()
        {
            return new ProductCategoryModel()
            {
                ID = -1,
                Name = "+ Nowa",
                Description = string.Empty
            };
        }
    }
}
