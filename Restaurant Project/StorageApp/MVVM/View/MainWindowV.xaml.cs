using StorageApp.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StorageApp.MVVM.View
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindowV.xaml
    /// </summary>
    public partial class MainWindowV : Window
    {
        public MainWindowV()
        {
            InitializeComponent();
            StartClock();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = new MainWindowVM(this);
            viewModel.RequestClose += () => Close();
            DataContext = viewModel;
        }

        private void StartClock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TickEvent;
            timer.Start();
        }

        private void TickEvent(object sender, EventArgs e)
        {
            ActiveClock.Content = DateTime.Now.ToString("g");
        }
    }
}
