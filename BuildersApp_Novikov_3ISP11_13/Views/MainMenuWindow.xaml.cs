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
using System.Windows.Shapes;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для MainMenuWindow.xaml
    /// </summary>
    public partial class MainMenuWindow : Window
    {

        Entities Entities = new Entities();
        public Post role;
        public MainMenuWindow()
        {
            InitializeComponent();
            MainWin = this;
            role = Entities.Post.Find(AuthWindow.authUser.Post.IdPost);
            if (role.PostName.Equals("Администратор"))
                tblockTitle.Text = "Окно администратора";
            else if(role.PostName.Equals("Менеджер"))
                tblockTitle.Text = "Окно менеджера";
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                MainWin.DragMove();
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
    }
}
