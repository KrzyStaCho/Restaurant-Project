using StorageApplication.MVVM.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowSelect
{
    class BaseWindowSelectVM<T> : BaseWindowVM
    {
        #region Private Fields

        private Func<string, List<T>> LoadFunc;
        private string searchBox = string.Empty;
        private List<T> list;

        #endregion
        #region Public Fields

        public T? SelectedObject;

        #endregion
        #region Data Binding

        public string SearchBox
        {
            get { return searchBox; }
            set
            {
                searchBox = value;
                OnPropertyChanged(nameof(SearchBox));
            }
        }

        public List<T> ObjectList
        {
            get { return list; }
            set
            {
                list = value;
                OnPropertyChanged(nameof(ObjectList));
            }
        }

        #endregion
        #region Commands

        public ICommand SearchData { get; }
        public ICommand ChooseObject { get; }

        private void ExecuteSearchData(object? parameter)
        {
            ObjectList = LoadFunc.Invoke(SearchBox);
        }
        private void ExecuteChooseObject(object? parameter)
        {
            if (parameter is T)
            {
                SelectedObject = (T)parameter;
                CloseCommand.Execute(null);
            }
        }

        #endregion
        #region Functions

        #endregion

        public BaseWindowSelectVM(Window window, string title, Func<string, List<T>> loadFunc)
            : base(window, title)
        {
            #region Init Commands

            SearchData = new BaseCommand(ExecuteSearchData);
            ChooseObject = new BaseCommand(ExecuteChooseObject);

            #endregion

            ObjectList = loadFunc.Invoke(string.Empty);
            LoadFunc = loadFunc;
        }
    }
}
