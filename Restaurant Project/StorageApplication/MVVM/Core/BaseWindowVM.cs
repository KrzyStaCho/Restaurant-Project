using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace StorageApplication.MVVM.Core
{
    class BaseWindowVM : BaseViewModel
    {
        protected Window mainWindow;

        #region Commands
        #region Close Command

        public ICommand CloseCommand { get; }
        public void ExecuteClose(object? parameter)
        {
            mainWindow.Close();
        }

        #endregion
        #endregion

        public BaseWindowVM(Window window, string title)
        {
            mainWindow = window;
            window.Title = title;

            mainWindow.DataContext = this;
            CloseCommand = new BaseCommand(ExecuteClose);
        }
    }
}
