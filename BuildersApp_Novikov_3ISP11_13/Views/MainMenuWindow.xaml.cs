using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Pages;
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

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {
        public MainMenuWindow()
        {
            InitializeComponent();
            winMain = this;
            if (AuthWindow.Role.Equals("Курьер"))
            {
                btnClients.Visibility = Visibility.Collapsed;
                btnEmployees.Visibility = Visibility.Collapsed;
                btnOrdersService.Visibility = Visibility.Collapsed;
                btnSC.Visibility = Visibility.Collapsed;
                btnSS.Visibility = Visibility.Collapsed;
                btnServices.Visibility = Visibility.Collapsed;
            }
            else if(AuthWindow.Role.Equals("Сантехник"))
            {
                btnComponents.Visibility = Visibility.Collapsed;
                btnClients.Visibility = Visibility.Collapsed;
                btnEmployees.Visibility = Visibility.Collapsed;
                btnOrdersComponent.Visibility = Visibility.Collapsed;
                btnSC.Visibility = Visibility.Collapsed;
                btnSS.Visibility = Visibility.Collapsed;
            }
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winMain.DragMove();
        }

        private void btnMinWindow_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void btnMaxWindow_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
                this.WindowState = WindowState.Maximized;

            else
                this.WindowState = WindowState.Normal;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void btnComponents_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new ComponentsPage());
        }

        private void btnOrdersComponent_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OrdersComponentPage());
        }

        private void btnEmployees_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new EmployeesPage());
        }

        private void btnClients_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new ClientsPage());
        }

        private void btnExitProfile_Click(object sender, RoutedEventArgs e)
        {
            AuthWindow authWindow = new AuthWindow();
            authWindow.Show();
            this.Close();
        }

        private void btnOpenSS_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new SellingServicesPage());
        }

        private void btnOpenSC_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new SellingComponentsPage());
        }

        private void btnServices_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new ServicePage());
        }

        private void btnOrdersService_Click(object sender, RoutedEventArgs e)
        {
            frmMain.Navigate(new OrdersServicePage());
        }
    }
}
