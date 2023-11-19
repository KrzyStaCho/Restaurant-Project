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
            //TODO: ...

            _menu = new List<ItemMenu>() { menu0 };
            OnPropertyChanged(nameof(Menu));
        }

        private BaseViewModel? FindInstanceVM<T>() where T : BaseViewModel
        {
            BaseViewModel? viewModel = viewModelList.FirstOrDefault(vm => vm is T);
            return viewModel;
        }

        #endregion

        public MainWindowVM(Window window)
            : base(window)
        {
            instance = this;
            DatabaseInstance = new RestaurantEntity();
            viewModelList = new List<BaseViewModel>();
            _activeUser = null;

            #region Init Commands

            SwitchToLogInView = new BaseCommand(ExecuteSwitchToLogIn);
            SwitchToDashboardView = new BaseCommand(ExecuteSwitchToDashboard, CanExecuteSwitchView);

            #endregion

            LoadMenuItem();
            SwitchToLogInView.Execute(null);
        }
    }
}