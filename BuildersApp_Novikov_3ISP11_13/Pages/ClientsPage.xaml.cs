using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для ClientsPage.xaml
    /// </summary>
    public partial class ClientsPage : Page
    {

        Entities Entities = new Entities();

        List<Client> listClient = new List<Client>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Фамилия (по возрастанию)",
            "Фамилия (по убыванию)",
            "Дата рождения (по возрастанию)",
            "Дата рождения (по убыванию)"
        };

        List<string> listFiltrGender = new List<string>();
        List<string> listFiltrIsDeleted = new List<string>();

        public ClientsPage()
        {
            InitializeComponent();

            LvClients.ItemsSource = Entities.Client.ToList();
            List<Gender> genders = Entities.Gender.ToList();
            List<Deleted> dels = Entities.Deleted.ToList();

            foreach (Deleted i in dels)
            {
                listFiltrIsDeleted.Add(i.DeletedName);
            }

            foreach (Gender i in genders)
            {
                listFiltrGender.Add(i.GenderName);
            }

            listFiltrGender.Insert(0, "Все категории");
            cboxFiltrGender.ItemsSource = listFiltrGender;
            cboxFiltrGender.SelectedIndex = 0;

            listFiltrIsDeleted.Insert(0, "Все категории");
            cboxFiltrIsDeleted.ItemsSource = listFiltrIsDeleted;
            cboxFiltrIsDeleted.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listClient = Entities.Client.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listClient = listClient.OrderBy(i => i.IdClient).ToList();
                    break;
                case 1:
                    listClient = listClient.OrderByDescending(i => i.IdClient).ToList();
                    break;
                case 2:
                    listClient = listClient.OrderBy(i => i.LastName).ToList();
                    break;
                case 3:
                    listClient = listClient.OrderByDescending(i => i.LastName).ToList();
                    break;
                case 4:
                    listClient = listClient.OrderBy(i => i.DateOfBirth).ToList();
                    break;
                case 5:
                    listClient = listClient.OrderByDescending(i => i.DateOfBirth).ToList();
                    break;
                default:
                    listClient = listClient.OrderBy(i => i.IdClient).ToList();
                    break;
            }

            if (cboxFiltrGender.SelectedIndex != 0)
            {
                listClient = listClient.Where(i => i.IdGender == cboxFiltrGender.SelectedIndex).ToList();
            }

            if (cboxFiltrIsDeleted.SelectedIndex != 0)
            {
                listClient = listClient.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            }

            listClient = listClient.Where(i => i.LastName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvClients.ItemsSource = listClient;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddClientWindow addClientWindow = new AddClientWindow();
            addClientWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvClients.SelectedItem != null)
            {
                AddClientWindow addClientWindow = new AddClientWindow((Client)LvClients.SelectedItem);
                addClientWindow.ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvClients.SelectedItem is Client client))
            {
                switch (MessageBox.Show("Выберите клиента.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление клиента?", "Удаление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (client.IdDeleted == 1)
                        MessageBox.Show("Клиент уже удален.", "Удаление клиента", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        client.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Клиент успешно удалён.", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvClients.ItemsSource = Entities.Client.ToList();
                        LvClients.Items.Refresh();
                    }
                }
            }
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrGender_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrIsDeleted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LvClients.Items.Refresh();
            LvClients.ItemsSource = Entities.Client.ToList();
        }
    }
}
