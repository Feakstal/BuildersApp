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
    /// Логика взаимодействия для OrderServiceWindow.xaml
    /// </summary>
    public partial class OrderServiceWindow : Window
    {

        Entities Entities = new Entities();
        private OrderService ord = new OrderService();
        public OrderServiceWindow(OrderService orderService)
        {
            if (orderService != null)
            {
                InitializeComponent();

                if (AuthWindow.Role.Equals("Сантехник"))
                {
                    btnConfirmOrder.Visibility = Visibility.Collapsed;
                    btnCompleteOrder.Visibility = Visibility.Collapsed;
                }

                OrderServiceWin = this;
                ord = orderService;
                tblockTitle.Text = "Заказ №" + orderService.IdOrderService;
                tblockService.Text = Entities.Service.Where(i => i.IdService == orderService.IdService).Select(i => i.ServiceName).FirstOrDefault();
                tblockLastNameCl.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.LastName).FirstOrDefault();
                tblockFirstNameCl.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.FirstName).FirstOrDefault();
                tblockFatherNameCl.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.FatherName).FirstOrDefault();
                tblockEmployee.Text = Entities.Employee.Where(i => i.IdEmployee == orderService.IdEmployee).Select(i => i.LastName).FirstOrDefault();
                tblockPayment.Text = Entities.Payment.Where(i => i.IdPayment == orderService.IdPayment).Select(i => i.PaymentName).FirstOrDefault();
                tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == orderService.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
                tblockAddress.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.Address).FirstOrDefault();
                tblockPhone.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.Phone).FirstOrDefault();
                tblockPerformanceDate.Text = orderService.PerformanceDate.ToString();

                if (ord.IdOrderStatus == 3)
                {
                    btnCompleteOrder.IsEnabled = false;
                }
                else if (ord.IdOrderStatus == 1)
                {
                    btnConfirmOrder.IsEnabled = false;
                }
                else if (ord.IdOrderStatus == 2 || ord.IdOrderStatus == 4)
                {
                    btnConfirmOrder.IsEnabled = false;
                    btnCompleteOrder.IsEnabled = false;
                }
            }
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                OrderServiceWin.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderService orderservice = Entities.OrderService.Find(ord.IdOrderService);
            orderservice.IdOrderStatus = 1;
            Entities.SaveChanges();
            tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == orderservice.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
            MessageBox.Show("Заказ успешно подтвержден.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            btnConfirmOrder.IsEnabled = false;
            btnCompleteOrder.IsEnabled = true;
        }

        private void btnCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderService orderservice = Entities.OrderService.Find(ord.IdOrderService);
            orderservice.IdOrderStatus = 2;

            if (Entities.SellingService.Where(i => i.IdService == orderservice.IdService).FirstOrDefault() == null)
            {
                SellingService sellingService = new SellingService
                {
                    IdService = Entities.Service.Where(i => i.IdService == orderservice.IdService).Select(i => i.IdService).FirstOrDefault(),
                    Quantity = 1,
                    SalesValue = Entities.Service.Where(i => i.IdService == orderservice.IdService).Select(i => i.Price).FirstOrDefault()
                };

                Entities.SellingService.Add(sellingService);
            }
            else if (Entities.SellingService.Where(i => i.IdService == orderservice.IdService).FirstOrDefault() != null)
            {
                var sellingService = Entities.SellingService.Where(i => i.IdService == orderservice.IdService).FirstOrDefault();
                sellingService.SalesValue += Entities.Service.Where(i => i.IdService == orderservice.IdService).Select(i => i.Price).FirstOrDefault();
                sellingService.Quantity += 1;
            }

            Entities.SaveChanges();
            tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == orderservice.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
            MessageBox.Show("Заказ успешно закрыт.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            btnConfirmOrder.IsEnabled = false;
            btnCompleteOrder.IsEnabled = false;
        }
    }
}
