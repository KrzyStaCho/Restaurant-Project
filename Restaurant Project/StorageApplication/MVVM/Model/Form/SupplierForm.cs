using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StorageApplication.MVVM.Model.Form
{
    class SupplierForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        private bool[] isValid = new bool[8];

        public Supplier? editedEntity { get; private set; }

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch(name)
                {
                    case nameof(CompanyName):
                        result = ValidationClass.CheckText(CompanyName, nameof(CompanyName));
                        isValid[0] = (result == null);
                        break;
                    case nameof(CompanyNIP):
                        result = ValidationClass.CheckNIP(CompanyNIP, nameof(CompanyNIP));
                        isValid[1] = (result == null);
                        break;
                    case nameof(Address):
                        result = ValidationClass.CheckText(Address, nameof(Address));
                        isValid[2] = (result == null);
                        break;
                    case nameof(City):
                        result = ValidationClass.CheckText(City, nameof(City));
                        isValid[3] = (result == null);
                        break;
                    case nameof(ContactName):
                        result = ValidationClass.CheckOptionalText(ContactName, nameof(ContactName));
                        isValid[4] = (result == null);
                        break;
                    case nameof(ContactTitle):
                        result = ValidationClass.CheckOptionalText(ContactTitle, nameof(ContactTitle));
                        isValid[5] = (result == null);
                        break;
                    case nameof(Phone):
                        result = ValidationClass.CheckOptionalPhone(Phone, nameof(Phone));
                        isValid[6] = (result == null);
                        break;
                    case nameof(HomePage):
                        result = ValidationClass.CheckOptionalText(HomePage, nameof(HomePage));
                        isValid[7] = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private string _companyName = string.Empty;
        public string CompanyName
        {
            get { return _companyName; }
            set
            {
                _companyName = value.Trim();
                OnPropertyChanged(nameof(CompanyName));
            }
        }
        private string _companyNIP = string.Empty;
        public string CompanyNIP
        {
            get { return _companyNIP; }
            set
            {
                _companyNIP = value.Trim();
                OnPropertyChanged(nameof(CompanyNIP));
            }
        }
        private string _address = string.Empty;
        public string Address
        {
            get { return _address; }
            set
            {
                _address = value.Trim();
                OnPropertyChanged(nameof(Address));
            }
        }
        private string _city = string.Empty;
        public string City
        {
            get { return _city; }
            set
            {
                _city = value.Trim();
                OnPropertyChanged(nameof(City));
            }
        }

        private string _contactName = string.Empty;
        public string ContactName
        {
            get { return _contactName; }
            set
            {
                _contactName = value.Trim();
                OnPropertyChanged(nameof(ContactName));
            }
        }
        private string _contactTitle = string.Empty;
        public string ContactTitle
        {
            get { return _contactTitle; }
            set
            {
                _contactTitle = value.Trim();
                OnPropertyChanged(nameof(ContactTitle));
            }
        }
        private string _phone = string.Empty;
        public string Phone
        {
            get { return _phone; }
            set
            {
                _phone = value.Trim();
                OnPropertyChanged(nameof(Phone));
            }
        }
        private string _homePage = string.Empty;
        public string HomePage
        {
            get { return _homePage; }
            set
            {
                _homePage = value.Trim();
                OnPropertyChanged(nameof(HomePage));
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

        #endregion
        #region Methods

        public void ClearInput()
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
        public void FillInput(Supplier model)
        {
            CompanyName = model.CompanyName;
            CompanyNIP = model.CompanyNip;
            Address = model.Address;
            City = model.City;

            ContactName = model.ContactName ?? string.Empty;
            ContactTitle = model.ContactTitle ?? string.Empty;
            Phone = model.Phone ?? string.Empty;
            HomePage = model.HomePage ?? string.Empty;

            editedEntity = model;
            Error = string.Empty;
        }
        public Supplier GetInput()
        {
            Supplier? filledSupplier = editedEntity;
            if (filledSupplier == null) filledSupplier = new Supplier();

            filledSupplier.CompanyName = CompanyName;
            filledSupplier.CompanyNip = CompanyNIP;
            filledSupplier.Address = Address;
            filledSupplier.City = City;

            filledSupplier.ContactName = (ValidationClass.IsEmpty(ContactName)) ? null : ContactName;
            filledSupplier.ContactTitle = (ValidationClass.IsEmpty(ContactTitle)) ? null : ContactTitle;
            filledSupplier.Phone = (ValidationClass.IsEmpty(Phone)) ? null : Phone;
            filledSupplier.HomePage = (ValidationClass.IsEmpty(HomePage)) ? null : HomePage;

            return filledSupplier;
        }

        public bool IsDataCorrect()
        {
            return isValid.All(v => v);
        }

        #endregion

        public SupplierForm()
        {
            editedEntity = null;
        }
    }
}
