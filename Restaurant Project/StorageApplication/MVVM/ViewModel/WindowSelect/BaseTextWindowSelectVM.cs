using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Model.Form;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel.WindowSelect
{
    class BaseTextWindowSelectVM : BaseWindowVM, IDataErrorInfo
    {
        #region Fields

        private Func<string, string, string?> ValFunc;

        private bool isValid = false;

        public FormStateEnum FormState;

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch (name)
                {
                    case nameof(Value):
                        result = ValFunc.Invoke(Value, nameof(Value));
                        isValid = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        public string Error => string.Empty;

        #region Data Binding

        public string HeaderText { get; }

        private string _value = string.Empty;
        public string Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged(nameof(Value));
            }
        }

        public ICommand ConfirmData { get; }

        #endregion

        #endregion

        #region Methods

        private void ExecuteConfirmData(object? parameter)
        {
            FormState = FormStateEnum.CONFIRM;
            CloseCommand.Execute(null);
        }

        private bool CanExecuteConfirmData(object? parameter)
        {
            return isValid;
        }

        #endregion

        public BaseTextWindowSelectVM(Window window, string title, string header, Func<string, string, string?> validateFunc)
            : base(window, title)
        {
            FormState = FormStateEnum.CANCEL;
            HeaderText = header;
            ValFunc = validateFunc;

            ConfirmData = new BaseCommand(ExecuteConfirmData, CanExecuteConfirmData);
        }
    }
}
