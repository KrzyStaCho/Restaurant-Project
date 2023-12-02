using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.Menu
{
    class ItemMenu
    {
        #region Fields

        public string Header { get; private set; }
        public PackIconKind Icon {  get; private set; }
        public List<SubItemMenu> SubItems { get; private set; }

        #endregion

        public ItemMenu(string header, PackIconKind icon, List<SubItemMenu> subItems)
        {
            Header = header;
            Icon = icon;
            SubItems = subItems;
        }
    }
}
