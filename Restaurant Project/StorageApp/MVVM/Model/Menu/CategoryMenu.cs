using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.Menu
{
    public class CategoryMenu
    {
        #region Fields

        public string Name { get; private set; }
        public int CategoryID { get; private set; }

        #endregion

        public CategoryMenu(string name, int categoryID)
        {
            Name = name;
            CategoryID = categoryID;
        }
    }
}
