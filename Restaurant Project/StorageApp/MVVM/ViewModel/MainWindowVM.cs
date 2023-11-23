using ProjectLibrary.Repository.Context;
using StorageApp.MVVM.Model;
using StorageApp.MVVM.Model.Menu;
using StorageApp.MVVM.View;
using StorageApp.MVVM.ViewModel.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace StorageApp.MVVM.ViewModel
{
    public class MainWindowVM : BaseWindowVM
    {
        #region Private Fields
        #region Static Fields

        private static MainWindowVM instance;

        private static readonly string CarrotIconName = "Solid_Carrot";
        private static readonly string HomeIconName = "Solid_Home";
        private static readonly string NewspaperIconName = "Regular_Newspaper";
        private static readonly string UserIconName = "Solid_User";
        private static readonly string LightIconName = "Regular_Lightbulb";

        #endregion

        private readonly string _version = "2.0";
        private ActiveUserModel? _activeUser;
        private int changesInDB;

        private List<BaseViewModel> viewModelList;
        private UserControl currentChildView;
        private List<ItemMenu> _menu;

        #region Getter Child ViewModel

        private LogInVM LogInViewModel
        {
            get
            {
                LogInVM? viewModel = FindInstanceVM<LogInVM>() as LogInVM;
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
                DashboardVM? viewModel = FindInstanceVM<DashboardVM>() as DashboardVM;
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
                SupplierVM? viewModel = FindInstanceVM<SupplierVM>() as SupplierVM;
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
                ProductVM? viewModel = FindInstanceVM<ProductVM>() as ProductVM;
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

        public RestaurantEntity DatabaseInstance;
        public static MainWindowVM Instance
        {
            get { return instance; }
        }
        public ActiveUserModel? ActiveUser
        {
            get { return _activeUser; }
            set
            {
                _activeUser = value;
                OnPropertyChanged(nameof(ActiveUser));
                OnPropertyChanged(nameof(ActiveUsername));
            }
        }

        #endregion
        #region Data Binding

        public string ActiveUsername
        {
            get
            {
                if (_activeUser == null) return "...";
                else return _activeUser.Username;
            }
        }
        public string FormatVersionText
        {
            get
            {
                return "Version " + _version;
            }
        }
        public string ChangesInDatabase
        {
            get
            {
                return "Changes in DB: " + changesInDB;
            }
        }
        public List<ItemMenu> Menu
        {
            get { return _menu; }
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

        //TODO: Remove after fill all menu
        private ICommand emptyCommand { get; set; }
        private void ExecuteEmpty(object? parameter) { }

        #region Switch ViewModel

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
            if (CurrentChildView != null && CurrentChildView is ProductV) return;
            var view = new ProductV();
            var viewModel = ProductViewModel;
            view.DataContext = viewModel;
            CurrentChildView = view;
        }

        private bool CanExecuteSwitchView(object? parameter)
        {
            return !(ActiveUser == null);
        }

        #endregion

        #endregion
        #region Functions

        private void LoadMenuItem()
        {
            //TODO: Remove after fill all menu
            emptyCommand = new BaseCommand(ExecuteEmpty);

            #region Start Menu

            List<SubItemMenu> menuStart = new List<SubItemMenu>()
            {
                new SubItemMenu("Log in", SwitchToLogInView),
                new SubItemMenu("Dashboard", SwitchToDashboardView)
            };
            ItemMenu menu0 = new ItemMenu("Start", menuStart, HomeIconName);

            #endregion
            #region Product Menu

            List<SubItemMenu> menuProduct = new List<SubItemMenu>()
            {
                new SubItemMenu("Product", SwitchToProductView)
            };
            ItemMenu menu1 = new ItemMenu("Storage", menuProduct, CarrotIconName);

            #endregion
            #region SupplyDelivery Menu

            List<SubItemMenu> menuDelivery = new List<SubItemMenu>()
            {
                new SubItemMenu("Suppliers", SwitchToSupplierView)
            };
            ItemMenu menu2 = new ItemMenu("Supply", menuDelivery, NewspaperIconName);

            #endregion

            _menu = new List<ItemMenu>() { menu0, menu1, menu2 };
            OnPropertyChanged(nameof(Menu));
        }

        private BaseViewModel? FindInstanceVM<T>() where T : BaseViewModel
        {
            BaseViewModel? viewModel = viewModelList.FirstOrDefault(vm => vm is T);
            return viewModel;
        }

        public Window GetWindow()
        {
            return mainWindow;
        }
        private void SetChangesInDB(int newInt)
        {
            changesInDB = newInt;
            OnPropertyChanged(nameof(ChangesInDatabase));
        }
        public void IncrementChangesInDB()
        {
            SetChangesInDB(changesInDB + 1);
        }

        #endregion

        public MainWindowVM(Window window)
            : base(window, "Storage Application")
        {
            instance = this;
            DatabaseInstance = new RestaurantEntity();
            viewModelList = new List<BaseViewModel>();
            _activeUser = null;
            changesInDB = 0;

            #region Init Commands

            SwitchToLogInView = new BaseCommand(ExecuteSwitchToLogIn);
            SwitchToDashboardView = new BaseCommand(ExecuteSwitchToDashboard, CanExecuteSwitchView);
            SwitchToSupplierView = new BaseCommand(ExecuteSwitchToSupplier, CanExecuteSwitchView);
            SwitchToProductView = new BaseCommand(ExecuteSwitchToProduct, CanExecuteSwitchView);

            #endregion

            LoadMenuItem();
            SwitchToLogInView.Execute(null);
        }
    }
}