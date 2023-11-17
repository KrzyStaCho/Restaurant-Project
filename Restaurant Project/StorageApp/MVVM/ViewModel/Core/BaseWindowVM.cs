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
        public event Action? RequestClose;

        private Window mainWindow;

        #region Commands
        #region Close Command

        public ICommand CloseCommand { get; }
        public void ExecuteClose(object? parameter)
        {
            if (RequestClose != null) { RequestClose(); }
        }

        #endregion
        #endregion

        public BaseWindowVM(Window window)
        {
            mainWindow = window;
            CloseCommand = new BaseCommand(ExecuteClose);
        }
    }
}
