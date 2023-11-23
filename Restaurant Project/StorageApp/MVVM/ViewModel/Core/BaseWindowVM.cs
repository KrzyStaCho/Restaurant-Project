using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel.Core
{
    public class BaseWindowVM : BaseViewModel
    {
        protected Window mainWindow;

        #region Data Binding

        public string WindowTitle { get; protected set; }

        #endregion

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
            WindowTitle = title;

            mainWindow.DataContext = this;
            CloseCommand = new BaseCommand(ExecuteClose);
        }
    }
}
