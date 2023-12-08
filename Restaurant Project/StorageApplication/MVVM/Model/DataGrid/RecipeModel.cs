using ProjectLibrary.Repository.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.DataGrid
{
    class RecipeModel
    {
        public int RecipeID { get; set; }
        public string RecipeName { get; set; }
        public string CategoryName { get; set; }
        public int Products {  get; set; }

        public RecipeModel(Recipe model)
        {
            RecipeID = model.RecipeId;
            RecipeName = model.RecipeName;
            CategoryName = model.Category?.CategoryName ?? string.Empty;
            Products = model.RecipeDetails.Count;
        }
    }
}
