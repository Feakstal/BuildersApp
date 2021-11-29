using BuildersApp_Novikov_3ISP11_13.Helper;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class AuthWindow : Window
    {

        Entities Entities = new Entities();
        public static User authUser;
        public static string Role;
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
            authUser = Entities.User.FirstOrDefault(i => i.Login == tboxLogin.Text && i.Password == tboxPassword.Text);
            Role = authUser.Post.PostName;

            if(authUser != null)
            {
                MessageBox.Show($"Вы авторизовались от лица '{Role}'", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                MainMenuWindow mainMenuWindow = new MainMenuWindow();
                mainMenuWindow.Show();
                Hide();
            }
            else
            {
                if(tboxPassword.Text.Length == 0 && tboxLogin.Text.Length != 0)
                    MessageBox.Show("Вы не ввели пароль.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(tboxPassword.Text.Length == 0 && tboxLogin.Text.Length == 0)
                    MessageBox.Show("Вы не заполнили данные для авторизации.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(tboxLogin.Text.Length != 0 && tboxPassword.Text.Length != 0)
                    MessageBox.Show("В доступе отказано. Проверьте правильность введенных данных.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
                else if(tboxLogin.Text.Length == 0 && tboxPassword.Text.Length != 0)
                    MessageBox.Show("Вы не ввели логин.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Error);
            } 
        }
        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                AuthWin.DragMove();
        }
    }
}
