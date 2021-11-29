using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для OrdersServicePage.xaml
    /// </summary>
    public partial class OrdersServicePage : Page
    {

        Entities Entities = new Entities();

        List<OrderService> listOrderService = new List<OrderService>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Услуга (по возрастанию)",
            "Услуга (по убыванию)",
            "Дата создания (по возрастанию)",
            "Дата создания (по убыванию)",
            "Дата выполнения услуг (по возрастанию)",
            "Дата выполнения услуг (по убыванию)"
        };

        List<string> listFiltrPayment = new List<string>();
        List<string> listFiltrIsDeleted = new List<string>();
        List<string> listFilterStatus = new List<string>();

        public OrdersServicePage()
        {
            InitializeComponent();

            if (AuthWindow.Role.Equals("Сантехник"))
            {
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
                btnCancelOrder.Visibility = Visibility.Collapsed;
                btnDelete.Visibility = Visibility.Collapsed;
            }

            List<Payment> payments = Entities.Payment.ToList();
            List<Deleted> dels = Entities.Deleted.ToList();
            List<OrderStatus> orderStatuses = Entities.OrderStatus.ToList();

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
            listOrderService = Entities.OrderService.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listOrderService = listOrderService.OrderBy(i => i.IdOrderService).ToList();
                    break;
                case 1:
                    listOrderService = listOrderService.OrderByDescending(i => i.IdOrderService).ToList();
                    break;
                case 2:
                    listOrderService = listOrderService.OrderBy(i => i.IdService).ToList();
                    break;
                case 3:
                    listOrderService = listOrderService.OrderByDescending(i => i.IdService).ToList();
                    break;
                case 6:
                    listOrderService = listOrderService.OrderBy(i => i.CreateDate).ToList();
                    break;
                case 7:
                    listOrderService = listOrderService.OrderByDescending(i => i.CreateDate).ToList();
                    break;
                case 8:
                    listOrderService = listOrderService.OrderBy(i => i.PerformanceDate).ToList();
                    break;
                case 9:
                    listOrderService = listOrderService.OrderByDescending(i => i.PerformanceDate).ToList();
                    break;
                default:
                    listOrderService = listOrderService.OrderBy(i => i.IdOrderService).ToList();
                    break;
            }

            if (cboxFiltrPayment.SelectedIndex != 0)
                listOrderService = listOrderService.Where(i => i.IdPayment == cboxFiltrPayment.SelectedIndex).ToList();
            
            if (cboxFiltrStatus.SelectedIndex != 0)
                listOrderService = listOrderService.Where(i => i.IdOrderStatus == cboxFiltrStatus.SelectedIndex).ToList();
            
            if (cboxFiltrIsDeleted.SelectedIndex != 0)
                listOrderService = listOrderService.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            
            listOrderService = listOrderService.Where(i => i.Service.ServiceName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvOrdersService.ItemsSource = listOrderService;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddOrderServiceWindow addOrderServiceWindow = new AddOrderServiceWindow();
            addOrderServiceWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersService.SelectedItem != null)
            {
                AddOrderServiceWindow addOrderServiceWindow = new AddOrderServiceWindow((OrderService)LvOrdersService.SelectedItem);
                addOrderServiceWindow.ShowDialog();
            }
            else
                MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvOrdersService.SelectedItem is OrderService orderService))
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
                    if (orderService.IdDeleted == 1)
                        MessageBox.Show("Заказ уже удален.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (orderService.IdOrderStatus != 4)
                        MessageBox.Show("Заказ находится в обработке, подтвержден или завершён.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        orderService.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Заказ успешно удалён.", "Удаление заказа", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvOrdersService.ItemsSource = Entities.OrderService.ToList();
                        LvOrdersService.Items.Refresh();
                    }
                }
            }
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
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
            LvOrdersService.Items.Refresh();
            LvOrdersService.ItemsSource = Entities.OrderService.ToList();
        }

        private void btnShowOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersService.SelectedItem != null)
            {
                OrderServiceWindow orderServiceWindow = new OrderServiceWindow((OrderService)LvOrdersService.SelectedItem);
                orderServiceWindow.ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnCancelOrder_Click(object sender, RoutedEventArgs e)
        {
            if (LvOrdersService.SelectedItem != null)
                CancelOrder((OrderService)LvOrdersService.SelectedItem);
            else MessageBox.Show("Вы не выбрали заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void CancelOrder(OrderService cancelOrder)
        {

            if ((cancelOrder != null && cancelOrder.IdOrderStatus == 3) || (cancelOrder != null && cancelOrder.IdOrderStatus == 1))
            {
                cancelOrder.IdOrderStatus = 4;
                Entities.SaveChanges();
                MessageBox.Show("Заказ успешно отменен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ((cancelOrder != null && cancelOrder.IdOrderStatus == 4) || (cancelOrder != null && cancelOrder.IdOrderStatus == 2))
            {
                MessageBox.Show("Заказ уже отменен или завершён.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }    
        }
    }
}
