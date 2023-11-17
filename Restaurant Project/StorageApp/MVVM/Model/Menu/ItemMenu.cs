using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.Menu
{
    public class ItemMenu
    {
        #region Fields

        public string Header { get; private set; }
        public string Icon { get; private set; }
        public List<SubItemMenu> SubItems { get; private set; }

        #endregion

        public ItemMenu(string header, List<SubItemMenu> subItems, string icon)
        {
            Header = header;
            Icon = icon;
            SubItems = subItems;
        }
    }
}
