using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApp.MVVM.Model.Item
{
    public class DashItem
    {
        #region Fields

        public string Icon { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }

        #endregion

        public DashItem(string icon, string title, string content)
        {
            Icon = icon;
            Title = title;
            Content = content;
        }
    }
}
