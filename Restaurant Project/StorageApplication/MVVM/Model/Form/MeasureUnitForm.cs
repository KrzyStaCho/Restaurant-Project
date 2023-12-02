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

namespace StorageApplication.MVVM.Model.Form
{
    class MeasureUnitForm : BaseProperty, IDataErrorInfo
    {
        #region Fields

        private bool isValid = false;

        private Action<int> LoadFunction;
        public MeasureUnit? ActionTarget { get; private set; }

        public string this[string name]
        {
            get
            {
                string? result = null;

                switch(name)
                {
                    case nameof(Code):
                        result = ValidationClass.CheckTextCode(Code, nameof(Code));
                        isValid = (result == null);
                        break;
                }

                return result ?? string.Empty;
            }
        }

        #region Data Binding

        private bool _isEnabled;
        public bool IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                _isEnabled = value;
                OnPropertyChanged(nameof(IsEnabled));
            }
        }

        private List<ObjectPair<int, string>> measureUnits;
        public List<ObjectPair<int, string>> UnitList
        {
            get { return measureUnits; }
            set
            {
                measureUnits = value;
                OnPropertyChanged(nameof(UnitList));
            }
        }
        private ObjectPair<int, string> _selected;
        public ObjectPair<int, string> SelectedItem
        {
            get { return _selected; }
            set
            {
                _selected = value;
                IsEnabled = (value != null);
                ClearInput();
                if (value != null && value.FirstItem != -1) LoadFunction.Invoke(value.FirstItem);
                OnPropertyChanged(nameof(SelectedItem));
            }
        }

        private string _code = string.Empty;
        public string Code
        {
            get { return _code; }
            set
            {
                _code = value.Trim();
                OnPropertyChanged(nameof(Code));
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
            Code = string.Empty;
            Error = string.Empty;
        }
        public void FillInput(MeasureUnit model)
        {
            Code = model.Code;
            ActionTarget = model;
            Error = string.Empty;
        }
        public MeasureUnit GetInput()
        {
            MeasureUnit? targetUnit = ActionTarget;
            if (targetUnit == null) targetUnit = new MeasureUnit();

            targetUnit.Code = Code;
            return targetUnit;
        }
        public bool IsDataCorrect()
        {
            if (!IsEnabled) return false;
            return isValid;
        }

        #endregion

        public MeasureUnitForm(List<ObjectPair<int, string>> unitList, Action<int> loadFunction)
        {
            UnitList = unitList;
            LoadFunction = loadFunction;
            ActionTarget = null;
        }
    }
}
