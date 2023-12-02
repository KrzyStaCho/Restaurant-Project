using ProjectLibrary.Repository.Entity;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Helper;
using StorageApplication.MVVM.Model.Item;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace StorageApplication.MVVM.Model.Form
{
    class ProductForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        private bool[] isValid = new bool[2];

        public Product? ActionTarget { get; private set; }

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch(name)
                {
                    case nameof(ProductName):
                        result = ValidationClass.CheckText(ProductName, nameof(ProductName));
                        isValid[0] = (result == null);
                        break;
                    case nameof(SelectedUnit):
                        result = ValidationClass.CheckComboBox(SelectedUnit, nameof(SelectedUnit));
                        isValid[1] = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private string _productName = string.Empty;
        public string ProductName
        {
            get { return _productName; }
            set
            {
                _productName = value.Trim();
                OnPropertyChanged(nameof(ProductName));
            }
        }

        private List<ObjectPair<int, string>> _unitList;
        public List<ObjectPair<int, string>> UnitList
        {
            get { return _unitList; }
            set
            {
                _unitList = value;
                OnPropertyChanged(nameof(UnitList));
            }
        }
        private ObjectPair<int, string>? _selectedUnit;
        public ObjectPair<int, string>? SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
                OnPropertyChanged(nameof(SelectedUnit));
            }
        }

        private ObjectPair<int, string> _selectedCategory;
        public ObjectPair<int, string> SelectedCategory
        {
            get { return _selectedCategory; }
            set
            {
                _selectedCategory = value;
                OnPropertyChanged(nameof(SelectedCategory));
            }
        }
        private ObjectPair<int, string> _selectedSupplier;
        public ObjectPair<int, string> SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
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

        public ICommand GetCategory { get; }
        public ICommand GetSupplier { get; }

        #endregion

        #endregion
        #region Methods

        public void ClearInput()
        {
            ProductName = string.Empty;
            SelectedUnit = null;

            SelectedCategory = ValidationClass.EmptyFKey;
            SelectedSupplier = ValidationClass.EmptyFKey;

            Error = string.Empty;
        }
        public void FillInput(Product model)
        {
            ProductName = model.ProductName;
            SelectedUnit = UnitList.FirstOrDefault(u => u.FirstItem == model.UnitId);

            if (model.Category == null) SelectedCategory = ValidationClass.EmptyFKey;
            else SelectedCategory = new ObjectPair<int, string>(model.Category.CategoryId, model.Category.CategoryName);

            if (model.Supplier == null) SelectedSupplier = ValidationClass.EmptyFKey;
            else SelectedSupplier = new ObjectPair<int, string>(model.Supplier.SupplierId, model.Supplier.CompanyName);

            Error = string.Empty;
            ActionTarget = model;
        }
        public Product GetInput()
        {
            if (SelectedUnit == null) throw new Exception();

            Product? filledProduct = ActionTarget;
            if (filledProduct == null) filledProduct = new Product();

            filledProduct.ProductName = ProductName;
            filledProduct.UnitId = SelectedUnit.FirstItem;
            filledProduct.CategoryId = (SelectedCategory.FirstItem == -1) ? null : SelectedCategory.FirstItem;
            filledProduct.SupplierId = (SelectedSupplier.FirstItem == -1) ? null : SelectedSupplier.FirstItem;

            return filledProduct;
        }
        public bool IsDataCorrect()
        {
            return isValid.All(v => v);
        }

        #endregion

        public ProductForm(List<ObjectPair<int, string>> unitList, ICommand getCategory, ICommand getSupplier)
        {
            UnitList = unitList;
            SelectedCategory = ValidationClass.EmptyFKey;
            SelectedSupplier = ValidationClass.EmptyFKey;

            GetCategory = getCategory;
            GetSupplier = getSupplier;
            ActionTarget = null;
        }
    }
}
