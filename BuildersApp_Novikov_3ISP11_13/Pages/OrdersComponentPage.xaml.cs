using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersPage.xaml
    /// </summary>
    public partial class OrdersComponentPage : Page
    {

        Entities Entities = new Entities();

        List<OrderComponent> listOrderComponent = new List<OrderComponent>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Компонент (по возрастанию)",
            "Компонент (по убыванию)",
            "Дата создания (по возрастанию)",
            "Дата создания (по убыванию)",
            "Дата доставки (по возрастанию)",
            "Дата доставки (по убыванию)"
        };

        List<string> listFiltrSupply = new List<string>();
        List<string> listFiltrPayment = new List<string>();
        List<string> listFiltrIsDeleted = new List<string>();
        List<string> listFilterStatus = new List<string>();

        public OrdersComponentPage()
        {
            InitializeComponent();

            if (AuthWindow.Role.Equals("Курьер"))
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnCancelOrder.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }

            List<Payment> payments = Entities.Payment.ToList();
            List<SupplierType> supplierTypes = Entities.SupplierType.ToList();
            List<Deleted> dels = Entities.Deleted.ToList();
            List<OrderStatus> orderStatuses = Entities.OrderStatus.ToList();

            foreach (SupplierType i in supplierTypes)
            {
                listFiltrSupply.Add(i.SupplierTypeName);
            }
            foreach (Payment i in payments)
            {
                listFiltrPayment.Add(i.PaymentName);
            }
            foreach (Deleted i in dels)
            {
                listFiltrIsDeleted.Add(i.DeletedName);
            }
            foreach (OrderStatus i in orderStatuses)
            {
                listFilterStatus.Add(i.OrderStatusName);
            }

            listFiltrSupply.Insert(0, "Все категории");
            cboxFiltrSupply.ItemsSource = listFiltrSupply;
            cboxFiltrSupply.SelectedIndex = 0;

            listFiltrPayment.Insert(0, "Все категории");
            cboxFiltrPayment.ItemsSource = listFiltrPayment;
            cboxFiltrPayment.SelectedIndex = 0;

            listFiltrIsDeleted.Insert(0, "Все категории");
            cboxFiltrIsDeleted.ItemsSource = listFiltrIsDeleted;
            cboxFiltrIsDeleted.SelectedIndex = 0;

            listFilterStatus.Insert(0, "Все категории");
            cboxFiltrStatus.ItemsSource = listFilterStatus;
            cboxFiltrStatus.SelectedIndex = 0;

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listOrderComponent = Entities.OrderComponent.ToList();

            switch (cboxSort.SelectedIndex)
            {
                case 0:
                    listOrderComponent = listOrderComponent.OrderBy(i => i.IdOrderComponent).ToList();
                    break;
                case 1:
                    listOrderComponent = listOrderComponent.OrderByDescending(i => i.IdOrderComponent).ToList();
                    break;
                case 2:
                    listOrderComponent = listOrderComponent.OrderBy(i => i.IdComponent).ToList();
                    break;
                case 3:
                    listOrderComponent = listOrderComponent.OrderByDescending(i => i.IdComponent).ToList();
                    break;
                case 6:
                    listOrderComponent = listOrderComponent.OrderBy(i => i.CreateDate).ToList();
                    break;
                case 7:
                    listOrderComponent = listOrderComponent.OrderByDescending(i => i.CreateDate).ToList();
                    break;
                case 8:
                    listOrderComponent = listOrderComponent.OrderBy(i => i.PerformanceDate).ToList();
                    break;
                case 9:
                    listOrderComponent = listOrderComponent.OrderByDescending(i => i.PerformanceDate).ToList();
                    break;
                default:
                    listOrderComponent = listOrderComponent.OrderBy(i => i.IdOrderComponent).ToList();
                    break;
            }

            if (cboxFiltrSupply.SelectedIndex != 0)
                listOrderComponent = listOrderComponent.Where(i => i.IdSupplierType == cboxFiltrSupply.SelectedIndex).ToList();
            
            if (cboxFiltrPayment.SelectedIndex != 0)
                listOrderComponent = listOrderComponent.Where(i => i.IdPayment == cboxFiltrPayment.SelectedIndex).ToList();
            
            if (cboxFiltrStatus.SelectedIndex != 0)
                listOrderComponent = listOrderComponent.Where(i => i.IdOrderStatus == cboxFiltrStatus.SelectedIndex).ToList();
            
            if (cboxFiltrIsDeleted.SelectedIndex != 0)
                listOrderComponent = listOrderComponent.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            
            listOrderComponent = listOrderComponent.Where(i => i.Component.ComponentName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvOrdersComponent.ItemsSource = listOrderComponent;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrderComponentWindow addOrderWindow = new AddOrderComponentWindow();
            addOrderWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersComponent.SelectedItem != null)
            {
                AddOrderComponentWindow addOrderWindow = new AddOrderComponentWindow((OrderComponent)LvOrdersComponent.SelectedItem);
                addOrderWindow.ShowDialog();
            }
            else
                MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvOrdersComponent.SelectedItem is OrderComponent orderComponent))
            {
                switch (MessageBox.Show("Выберите заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление заказа?", "Удаление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if (orderComponent.IdDeleted == 1)
                        MessageBox.Show("Заказ уже удален.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (orderComponent.IdOrderStatus != 4)
                        MessageBox.Show("Заказ находится в обработке, подтвержден или завершён.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        orderComponent.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Заказ успешно удалён.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvOrdersComponent.ItemsSource = Entities.OrderComponent.ToList();
                        LvOrdersComponent.Items.Refresh();
                    }
                }
            }
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrSupply_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrPayment_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrIsDeleted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LvOrdersComponent.Items.Refresh();
            LvOrdersComponent.ItemsSource = Entities.OrderComponent.ToList();
        }

        private void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersComponent.SelectedItem != null)
            {
                OrderComponentWindow orderComponentWindow = new OrderComponentWindow((OrderComponent)LvOrdersComponent.SelectedItem);
                orderComponentWindow.ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersComponent.SelectedItem != null)
                CancelOrder((OrderComponent)LvOrdersComponent.SelectedItem);
            else MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CancelOrder(OrderComponent cancelOrder)
        {

            if ((cancelOrder != null && cancelOrder.IdOrderStatus == 3) || (cancelOrder != null && cancelOrder.IdOrderStatus == 1))
            {
                cancelOrder.IdOrderStatus = 4;
                Entities.SaveChanges();
                MessageBox.Show("Заказ успешно отменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((cancelOrder != null && cancelOrder.IdOrderStatus == 4) || (cancelOrder != null && cancelOrder.IdOrderStatus == 2))
            {
                MessageBox.Show("Заказ уже отменён или завершён.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
