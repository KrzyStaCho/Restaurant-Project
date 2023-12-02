using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApplication.MVVM.Model.Form
{
    class LogInForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        #region Error Message

        public static readonly string IncorrectDataError = "There is no account with this username and password";

        #endregion

        private bool[] isValid = new bool[2];

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch(name)
                {
                    case nameof(Username):
                        result = ValidationClass.CheckText(Username, nameof(Username));
                        isValid[0] = (result == null);
                        break;
                    case nameof(Password):
                        result = ValidationClass.CheckText(Password, nameof(Password));
                        isValid[1] = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private string _username = string.Empty;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value.Trim();
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password = string.Empty;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value.Trim();
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _error = string.Empty;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        #endregion

        public ICommand ConfirmCommand { get; private set; }

        #endregion
        #region Methods

        public void ClearInput()
        {
            Username = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
        }
        public bool IsDataCorrect()
        {
            return isValid.All(v => v);
        }
        public string GetHashedPassword()
        {
            SHA256 hash = SHA256.Create();
            byte[] passwordBytes = Encoding.Default.GetBytes(Password);
            byte[] hashedPass = hash.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedPass);
        }

        #endregion

        public LogInForm(ICommand command)
        {
            ConfirmCommand = command;
        }
    }
}
