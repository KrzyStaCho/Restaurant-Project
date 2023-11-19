using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApp.MVVM.Model.Form
{
    public class LogInForm : BaseProperty
    {
        #region Fields

        #region Static Fields

        public static readonly string EmptyBoxesError = "Username and Password cannot be empty";
        public static readonly string NotEnoughLettersError = "Username and Password must have 3 or more letters";
        public static readonly string IncorrectDataError = "There is no account with this Username and Password";

        #endregion

        private string _username;
        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged(nameof(Username));
            }
        }

        private string _password;
        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged(nameof(Password));
            }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        public ICommand ConfirmCommand { get; private set; }

        #endregion
        #region Functions

        public void Clear()
        {
            Username = string.Empty;
            Password = string.Empty;
            Error = string.Empty;
        }
        public bool IsDataEmpty()
        {
            if (string.IsNullOrWhiteSpace(Username)) return true;
            if (string.IsNullOrWhiteSpace(Password)) return true;
            return false;
        }
        public bool CheckData()
        {
            //1. Data cannot be empty, null or only white characters
            if (IsDataEmpty()) { Error = EmptyBoxesError; return false; }

            //2. Data must have 3 or more letters
            if (Username.Length < 3 || Password.Length < 3) { Error = NotEnoughLettersError; return false; }

            return true;
        }
        public void TrimData()
        {
            Username = Username.Trim();
            Password = Password.Trim();
        }
        public string GetHashedPassword()
        {
            SHA256 hash = SHA256.Create();
            byte[] passwordBytes = Encoding.Default.GetBytes(Password);
            byte[] hashedPass = hash.ComputeHash(passwordBytes);
            return Convert.ToBase64String(hashedPass);
        }

        #endregion

        public LogInForm(ICommand confirmCommand)
        {
            Clear();
            ConfirmCommand = confirmCommand;
        }
    }
}
