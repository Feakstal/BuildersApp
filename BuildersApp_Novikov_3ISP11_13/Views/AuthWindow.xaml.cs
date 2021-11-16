using BuildersApp_Novikov_3ISP11_13.Helper;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {

        Entities Entities = new Entities();
        public static User authUser;
        public AuthWindow()
        {
            InitializeComponent();
            AuthWin = this;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void BtnEnter_Click(object sender, RoutedEventArgs e)
        {
            authUser = Entities.User.FirstOrDefault(i => i.Login == tboxLogin.Text && i.Password.Equals(tboxPassword.Text));
            if(authUser != null)
            {
                if (authUser.Password == tboxPassword.Text)
                {
                    MainMenuWindow mainMenuWindow = new MainMenuWindow();
                    mainMenuWindow.Show();
                    Hide();
                }
                else
                {
                    MessageBox.Show("Неверный пароль, повторите попытку", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }    
            }
            else
            {
                MessageBox.Show("Пользователь не найден, повторите попытку", "Пользователь не найден", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                AuthWin.DragMove();
        }
    }
}
