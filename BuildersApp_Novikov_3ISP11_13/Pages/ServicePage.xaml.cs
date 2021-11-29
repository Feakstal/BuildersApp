using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для ServicePage.xaml
    /// </summary>
    public partial class ServicePage : Page
    {
        Entities Entities = new Entities();

        List<Service> listService = new List<Service>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Услуга (по возрастанию)",
            "Услуга (по убыванию)",
            "Цена (по возрастанию)",
            "Цена (по убыванию)"
        };

        List<string> listFiltrIsDeleted = new List<string>();
        public ServicePage()
        {
            InitializeComponent();

            if (AuthWindow.Role.Equals("Сантехник"))
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }
            else if (AuthWindow.Role.Equals("Менеджер"))
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }

            LvServices.ItemsSource = Entities.Service.ToList();
            List<Deleted> deleteds = Entities.Deleted.ToList();
            foreach (Deleted i in deleteds)
            {
                listFiltrIsDeleted.Add(i.DeletedName);
            }

            listFiltrIsDeleted.Insert(0, "Все категории");
            cboxFiltrIsDeleted.ItemsSource = listFiltrIsDeleted;
            cboxFiltrIsDeleted.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listService = Entities.Service.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listService = listService.OrderBy(i => i.IdService).ToList();
                    break;
                case 1:
                    listService = listService.OrderByDescending(i => i.IdService).ToList();
                    break;
                case 2:
                    listService = listService.OrderBy(i => i.ServiceName).ToList();
                    break;
                case 3:
                    listService = listService.OrderByDescending(i => i.ServiceName).ToList();
                    break;
                case 4:
                    listService = listService.OrderBy(i => i.Price).ToList();
                    break;
                case 5:
                    listService = listService.OrderByDescending(i => i.Price).ToList();
                    break;
                default:
                    listService = listService.OrderBy(i => i.IdService).ToList();
                    break;
            }

            int selectFiltr = cboxFiltrIsDeleted.SelectedIndex;

            if (selectFiltr != 0)
            {
                listService = listService.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            }

            listService = listService.Where(i => i.ServiceName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvServices.ItemsSource = listService;
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LvServices.Items.Refresh();
            LvServices.ItemsSource = Entities.Service.ToList();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddServiceWindow addServiceWindow = new AddServiceWindow();
            addServiceWindow.Show();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvServices.SelectedItem != null)
            {
                AddServiceWindow addServiceWindow = new AddServiceWindow((Service)LvServices.SelectedItem);
                addServiceWindow.Show();
            }
            else MessageBox.Show("Вы не выбрали услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvServices.SelectedItem is Service service))
            {
                switch (MessageBox.Show("Выберите услугу.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление услуги?", "Удаление услуги", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (service.IdDeleted == 1)
                    {
                        MessageBox.Show("Услуга уже удалена.", "Удаление услуги", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        service.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Услуга успешно удалена.", "Удаление услуги", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvServices.ItemsSource = Entities.Service.ToList();
                        LvServices.Items.Refresh();
                    }
                }
            }
        }

        private void cboxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }
    }
}
