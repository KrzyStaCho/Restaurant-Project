using ProjectLibrary.Repository.Context;
using StorageApplication.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Core
{
    class BaseChildViewModel : BaseViewModel
    {
        protected MainWindowVM mainVM;
        protected RestaurantEntity database;

        public BaseChildViewModel()
        {
            mainVM = MainWindowVM.Instance;
            database = MainWindowVM.DatabaseInstance;
        }
    }
}
