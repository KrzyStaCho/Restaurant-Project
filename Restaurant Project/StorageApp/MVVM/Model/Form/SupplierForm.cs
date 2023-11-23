using ProjectLibrary.Repository.Entity;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApp.MVVM.Model.Form
{
    public class SupplierForm : BaseProperty
    {
        #region Fields
        #region Static Fields

        public static readonly string EmptyBoxesError = "All data from 'Default Tab' cannot be empty!";
        public static readonly string NotEnoughtLetterStandartError = "must have more than 2 character";
        public static readonly string NotEnoightCharNipError = "Company NIP must have 10 digits";
        public static readonly string OnlyDigitNipError = "Company NIP must have only digits";
        public static readonly string IncorrectPhone = "Phone must be write like example => (+48)456723189";

        public static readonly string PhoneRegexPattern = @"^\(\+\d{1,2}\)\d{9}$";

        #endregion

        public Supplier? EditedSupplier { get; private set; }

        #region Data Binding

        private string _companyName;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value;
                OnPropertyChanged(nameof(CompanyName));
            }
        }

        private string _companyNIP;
        public string CompanyNIP
        {
            get { return _companyNIP; }
            set
            {
                _companyNIP = value;
                OnPropertyChanged(nameof(CompanyNIP));
            }
        }

        private string _address;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value;
                OnPropertyChanged(nameof(Address));
            }
        }

        private string _city;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value;
                OnPropertyChanged(nameof(City));
            }
        }

        private string _contactName;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value;
                OnPropertyChanged(nameof(ContactName));
            }
        }

        private string _contactTitle;
        public string ContactTitle
        {
            get { return _contactTitle; }
            set
            {
                _contactTitle = value;
                OnPropertyChanged(nameof(ContactTitle));
            }
        }

        private string _phone;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value;
                OnPropertyChanged(nameof(Phone));
            }
        }

        private string _homePage;
        public string HomePage
        {
            get { return _homePage; }
            set
            {
                _homePage = value;
                OnPropertyChanged(nameof(HomePage));
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

        #endregion

        #endregion

        #region Functions

        public void Clear()
        {
            CompanyName = string.Empty;
            CompanyNIP = string.Empty;
            Address = string.Empty;
            City = string.Empty;
            ContactName = string.Empty;
            ContactTitle = string.Empty;
            Phone = string.Empty;
            HomePage = string.Empty;
            Error = string.Empty;
        }
        public void TrimData()
        {
            CompanyName = CompanyName.Trim();
            CompanyNIP = CompanyNIP.Trim();
            Address = Address.Trim();
            City = City.Trim();
            ContactName = ContactName.Trim();
            ContactTitle = ContactTitle.Trim();
            Phone = Phone.Trim();
            HomePage = HomePage.Trim();
        }
        public void FillData(Supplier model)
        {
            CompanyName = model.CompanyName;
            CompanyNIP = model.CompanyNip;
            Address = model.Address;
            City = model.City;
            ContactName = (model.ContactName == null) ? string.Empty : model.ContactName;
            ContactTitle = (model.ContactTitle == null) ? string.Empty : model.ContactTitle;
            Phone = (model.Phone == null) ? string.Empty : model.Phone;
            HomePage = (model.HomePage == null) ? string.Empty : model.HomePage;
            EditedSupplier = model;
        }
        public bool IsDataEmpty()
        {
            if (string.IsNullOrWhiteSpace(CompanyName)) return true;
            if (string.IsNullOrWhiteSpace(CompanyNIP)) return true;
            if (string.IsNullOrWhiteSpace(Address)) return true;
            if (string.IsNullOrWhiteSpace(City)) return true;
            return false;
        }
        public bool CheckData()
        {
            //Check if urgent data is not empty
            if (IsDataEmpty()) { Error = EmptyBoxesError; return false; }
            
            //Check if CompanyName, Address, City has more than 2 letters
            if (CompanyName.Length < 3) { Error = "Company Name " + NotEnoughtLetterStandartError; return false; }
            if (Address.Length < 3) { Error = "Address " + NotEnoughtLetterStandartError; return false; }
            if (City.Length < 3) { Error = "City " + NotEnoughtLetterStandartError; return false; }

            //Validate CompanyNIP
            if (CompanyNIP.Length != 10) { Error = NotEnoightCharNipError; return false; }
            if (!CompanyNIP.All(char.IsDigit)) { Error = OnlyDigitNipError; return false; }

            //Check Phone
            if (!IsValidPhone()) { Error = IncorrectPhone; return false; }

            Error = string.Empty;
            return true;
        }
        public bool IsValidPhone()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(Phone)) return true;
                Regex regex = new Regex(PhoneRegexPattern);
                return regex.IsMatch(Phone);
            }
            catch (Exception) { return false; }
        }
        public Supplier GetFilledSupplier()
        {
            Supplier? targetSupplier = EditedSupplier;
            if (targetSupplier == null) targetSupplier = new Supplier();

            //Fill urgent data
            targetSupplier.CompanyName = CompanyName;
            targetSupplier.CompanyNip = CompanyNIP;
            targetSupplier.Address = Address;
            targetSupplier.City = City;

            //Fill optional data
            targetSupplier.ContactName = (string.IsNullOrWhiteSpace(ContactName)) ? null : ContactName;
            targetSupplier.ContactTitle = (string.IsNullOrWhiteSpace(ContactTitle)) ? null : ContactTitle;
            targetSupplier.Phone = (string.IsNullOrWhiteSpace(Phone)) ? null : Phone;
            targetSupplier.HomePage = (string.IsNullOrWhiteSpace(HomePage)) ? null : HomePage;

            return targetSupplier;
        }

        #endregion

        public SupplierForm()
        {
            Clear();
        }
    }
}
