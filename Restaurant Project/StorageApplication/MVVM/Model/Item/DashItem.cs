using FontAwesome5;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.Item
{
    class DashItem
    {
        #region Fields

        public EFontAwesomeIcon Icon { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        #endregion

        public DashItem(EFontAwesomeIcon icon, string title, string content)
        {
            Icon = icon;
            Title = title;
            Content = content;
        }
    }
}
