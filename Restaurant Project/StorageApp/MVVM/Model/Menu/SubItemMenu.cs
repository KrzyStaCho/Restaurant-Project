using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApp.MVVM.Model.Menu
{
    public class SubItemMenu
    {
        #region Fields

        public string Name { get; private set; }
        public ICommand Command { get; private set; }

        #endregion

        public SubItemMenu(string name, ICommand command)
        {
            Name = name;
            Command = command;
        }
    }
}
