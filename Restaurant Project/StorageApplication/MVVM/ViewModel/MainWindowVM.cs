using MaterialDesignThemes.Wpf;
using ProjectLibrary.Repository.Context;
using StorageApplication.MVVM.Core;
using StorageApplication.MVVM.Model;
using StorageApplication.MVVM.Model.Menu;
using StorageApplication.MVVM.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StorageApplication.MVVM.ViewModel
{
    class MainWindowVM : BaseWindowVM
    {
        #region Private Fields

        private readonly string _version = "3.1";
        private ActiveUserModel? _activeUser;
        private int changesInDB;

        private List<BaseViewModel> viewModelList;
        private UserControl currentChildView;
        private List<ItemMenu> navSideMenu;

        #region Getter Child ViewModel

        private LogInVM LogInViewModel
        {
            get
            {
                LogInVM? viewModel = FindInstanceVM<LogInVM>();
                if (viewModel == null)
                {
                    viewModel = new LogInVM();
                    viewModelList.Add(viewModel);
                }
                return viewModel;
            }
        }
        private DashboardVM DashboardViewModel
        {
            get
            {
                DashboardVM? viewModel = FindInstanceVM<DashboardVM>();
                if (viewModel == null)
                {
                    viewModel = new DashboardVM();
                    viewModelList.Add(viewModel);
                }
                return viewModel;
            }
        }
        private SupplierVM SupplierViewModel
        {
            get
            {
                SupplierVM? viewModel = FindInstanceVM<SupplierVM>();
                if (viewModel == null)
                {
                    viewModel = new SupplierVM();
                    viewModelList.Add(viewModel);
                }
                return viewModel;
            }
        }
        private ProductVM ProductViewModel
        {
            get
            {
                ProductVM? viewModel = FindInstanceVM<ProductVM>();
                if (viewModel == null)
                {
                    viewModel = new ProductVM();
                    viewModelList.Add(viewModel);
                }
                return viewModel;
            }
        }

        #endregion

        #endregion
        #region Public Fields

        public static RestaurantEntity DatabaseInstance { get; private set; }
        public static MainWindowVM Instance { get; private set; }

        public ActiveUserModel? ActiveUser
        {
            get {  return _activeUser; }
            set
            {
                _activeUser = value;
                OnPropertyChanged(nameof(ActiveUser));
                OnPropertyChanged(nameof(ActiveUsername));
            }
        }

        #endregion
        #region Data Binding

        public string FormatVersion
        {
            get { return "Version " + _version; }
        }
        public string ChangesInDB
        {
            get { return "Changes in DB: " + changesInDB; }
        }
        public string ActiveUsername
        {
            get
            {
                if (_activeUser == null) return "...";
                else return _activeUser.Username;
            }
        }
        public List<ItemMenu> NavSideMenu
        {
            get { return navSideMenu; }
        }
        public UserControl CurrentChildView
        {
            get { return currentChildView; }
            set
            {
                currentChildView = value;
                OnPropertyChanged(nameof(CurrentChildView));
            }
        }

        #endregion
        #region Commands

        #region Switch Child ViewModel

        public ICommand SwitchToLogInView { get; }
        public ICommand SwitchToDashboardView { get; }
        public ICommand SwitchToSupplierView { get; }
        public ICommand SwitchToProductView { get; }

        private void ExecuteSwitchToLogIn(object? parameter)
        {
            if (CurrentChildView != null && CurrentChildView is LogInV) return;
            var view = new LogInV();
            var viewModel = LogInViewModel;
            view.DataContext = viewModel;
            CurrentChildView = view;

            if (ActiveUser != null) { ActiveUser = null; }
        }
        private void ExecuteSwitchToDashboard(object? parameter)
        {
            if (CurrentChildView != null && CurrentChildView is DashboardV) return;
            var view = new DashboardV();
            var viewModel = DashboardViewModel;
            viewModel.LoadData();
            view.DataContext = viewModel;
            CurrentChildView = view;
        }
        private void ExecuteSwitchToSupplier(object? parameter)
        {
            if (CurrentChildView != null && CurrentChildView is SupplierV) return;
            var view = new SupplierV();
            var viewModel = SupplierViewModel;
            view.DataContext = viewModel;
            CurrentChildView = view;
        }
        private void ExecuteSwitchToProduct(object? parameter)
        {
            if (CurrentChildView != null && CurrentChildView is  ProductV) return;
            var view = new ProductV();
            var viewModel = ProductViewModel;
            view.DataContext = viewModel;
            CurrentChildView = view;
        }

        private bool CanExecuteSwitchView(object? parameter)
        {
            return (ActiveUser != null);
        }

        #endregion

        #endregion
        #region Functions

        private void LoadNavSideMenu()
        {
            List<SubItemMenu> tmpList;
            List<ItemMenu> itemMenus = new List<ItemMenu>();

            #region Start Menu

            tmpList = new List<SubItemMenu>()
            {
                new SubItemMenu("Log in", SwitchToLogInView),
                new SubItemMenu("Dashboard", SwitchToDashboardView)
            };
            itemMenus.Add(new ItemMenu("Start", PackIconKind.Home, tmpList));

            #endregion
            #region Product Menu

            tmpList = new List<SubItemMenu>()
            {
                new SubItemMenu("Products", SwitchToProductView)
            };
            itemMenus.Add(new ItemMenu("Storage", PackIconKind.Archive, tmpList));

            #endregion
            #region Delivery Menu

            tmpList = new List<SubItemMenu>()
            {
                new SubItemMenu("Suppliers", SwitchToSupplierView)
            };
            itemMenus.Add(new ItemMenu("Supply", PackIconKind.TruckDeliveryOutline, tmpList));

            #endregion

            navSideMenu = itemMenus;
            OnPropertyChanged(nameof(NavSideMenu));
        }
        private T? FindInstanceVM<T>() where T : BaseViewModel
        {
            return viewModelList.FirstOrDefault(vm => vm is T) as T;
        }
        
        public Window SetOwner(Window window)
        {
            window.Owner = mainWindow;
            return window;
        }

        private void SetChangesInDB(int newInt)
        {
            changesInDB = newInt;
            OnPropertyChanged(nameof(ChangesInDB));
        }
        public void IncrementChangesInDB()
        {
            SetChangesInDB(changesInDB + 1);
        }

        #endregion

        public MainWindowVM(Window window)
            : base(window, "Storage Application")
        {
            Instance = this;
            DatabaseInstance = new RestaurantEntity();
            viewModelList = new List<BaseViewModel>();
            _activeUser = null;

            #region Init Commands

            SwitchToLogInView = new BaseCommand(ExecuteSwitchToLogIn);
            SwitchToDashboardView = new BaseCommand(ExecuteSwitchToDashboard, CanExecuteSwitchView);
            SwitchToSupplierView = new BaseCommand(ExecuteSwitchToSupplier, CanExecuteSwitchView);
            SwitchToProductView = new BaseCommand(ExecuteSwitchToProduct, CanExecuteSwitchView);

            #endregion

            LoadNavSideMenu();
            SwitchToLogInView.Execute(null);
        }
    }
}
