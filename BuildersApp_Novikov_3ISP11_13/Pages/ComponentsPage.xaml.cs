using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для ComponentsPage.xaml
    /// </summary>
    public partial class ComponentsPage : Page
    {

        Entities Entities = new Entities();

        List<Component> listComponent = new List<Component>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Компонент (по возрастанию)",
            "Компонент (по убыванию)",
            "Цена (по возрастанию)",
            "Цена (по убыванию)"
        };

        List<string> listFiltrIsDeleted = new List<string>();
        public ComponentsPage()
        {
            InitializeComponent();


            if (AuthWindow.Role.Equals("Курьер"))
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }
            else if (AuthWindow.Role.Equals("Менеджер"))
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }

                LvComponents.ItemsSource = Entities.Component.ToList();
            List<Deleted> dels = Entities.Deleted.ToList();
            foreach(Deleted i in dels)
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
            listComponent = Entities.Component.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listComponent = listComponent.OrderBy(i => i.IdComponent).ToList();
                    break;
                case 1:
                    listComponent = listComponent.OrderByDescending(i => i.IdComponent).ToList();
                    break;
                case 2:
                    listComponent = listComponent.OrderBy(i => i.ComponentName).ToList();
                    break;
                case 3:
                    listComponent = listComponent.OrderByDescending(i => i.ComponentName).ToList();
                    break;
                case 4:
                    listComponent = listComponent.OrderBy(i => i.Price).ToList();
                    break;
                case 5:
                    listComponent = listComponent.OrderByDescending(i => i.Price).ToList();
                    break;
                default:
                    listComponent = listComponent.OrderBy(i => i.IdComponent).ToList();
                    break;
            }

            if (cboxFiltrIsDeleted.SelectedIndex != 0)
            {
                listComponent = listComponent.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            }

            listComponent = listComponent.Where(i => i.ComponentName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvComponents.ItemsSource = listComponent;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddComponentWindow addComponentWindow = new AddComponentWindow();
            addComponentWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvComponents.SelectedItem != null)
            {
                AddComponentWindow addComponentWindow = new AddComponentWindow((Component)LvComponents.SelectedItem);
                addComponentWindow.ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvComponents.SelectedItem is Client client))
            {
                switch (MessageBox.Show("Выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление товара?", "Удаление товара", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (client.IdDeleted == 1)
                    {
                        MessageBox.Show("Товар уже удален.", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        client.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Товар успешно удалён.", "Удаление товара", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvComponents.ItemsSource = Entities.Client.ToList();
                        LvComponents.Items.Refresh();
                    }
                }
            }
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltr_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LvComponents.Items.Refresh();
            LvComponents.ItemsSource = Entities.Component.ToList();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvComponents.SelectedItem is Component component))
            {
                switch (MessageBox.Show("Выберите товар.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление товара?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (component.IdDeleted == 1)
                    {
                        MessageBox.Show("Товар уже удалены.", "Удаление сотрудника", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                    else
                    {
                        component.IdDeleted = 1;
                        Entities.SaveChanges();
                        LvComponents.ItemsSource = Entities.Component.ToList();
                        LvComponents.Items.Refresh();
                    }
                }
            }
        }
    }
}
