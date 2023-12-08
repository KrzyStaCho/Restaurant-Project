using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.DataGrid
{
    class RecipeDetailModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public decimal Quantity { get; set; }

        public RecipeDetailModel(Product model, decimal quantity)
        {
            ProductID = model.ProductId;
            ProductName = model.ProductName;
            Quantity = quantity;
        }
        public RecipeDetailModel(RecipeDetail model)
        {
            ProductID = model.ProductId;
            ProductName = model.Product.ProductName;
            Quantity = model.Quantity;
        }
    }
}
