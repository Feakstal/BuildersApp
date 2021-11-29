using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для OrderWindow.xaml
    /// </summary>
    public partial class OrderComponentWindow : Window
    {

        Entities Entities = new Entities();
        private OrderComponent ord = new OrderComponent();
        public OrderComponentWindow(OrderComponent order)
        {
            if(order != null)
            {
                InitializeComponent();

                if (AuthWindow.Role.Equals("Курьер"))
                {
                    btnCompleteOrder.Visibility = Visibility.Collapsed;
                    btnConfirmOrder.Visibility = Visibility.Collapsed;
                }

                OrderComponentWin = this;
                ord = order;

                tblockTitle.Text = "Заказ №" + order.IdOrderComponent;
                tblockComponent.Text = Entities.Component.Where(i => i.IdComponent == order.IdComponent).Select(i => i.ComponentName).FirstOrDefault();
                tblockLastNameCl.Text = Entities.Client.Where(i => i.IdClient == order.IdClient).Select(i => i.LastName).FirstOrDefault();
                tblockFirstNameCl.Text = Entities.Client.Where(i => i.IdClient == order.IdClient).Select(i => i.FirstName).FirstOrDefault();
                tblockFatherNameCl.Text = Entities.Client.Where(i => i.IdClient == order.IdClient).Select(i => i.FatherName).FirstOrDefault();
                tblockEmployee.Text = Entities.Employee.Where(i => i.IdEmployee == order.IdEmployee).Select(i => i.LastName).FirstOrDefault();
                tblockPayment.Text = Entities.Payment.Where(i => i.IdPayment == order.IdPayment).Select(i => i.PaymentName).FirstOrDefault();
                tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == order.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
                tblockAddress.Text = Entities.Client.Where(i => i.IdClient == order.IdClient).Select(i => i.Address).FirstOrDefault();
                tblockPhone.Text = Entities.Client.Where(i => i.IdClient == order.IdClient).Select(i => i.Phone).FirstOrDefault();
                tblockPerformanceDate.Text = order.PerformanceDate.ToString();

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
                OrderComponentWin.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btnConfirmOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderComponent ordercomponent = Entities.OrderComponent.Find(ord.IdOrderComponent);
            ordercomponent.IdOrderStatus = 1;
            Entities.SaveChanges();
            tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == ordercomponent.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
            MessageBox.Show("Заказ успешно подтвержден.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            btnConfirmOrder.IsEnabled = false;
            btnCompleteOrder.IsEnabled = true;
        }

        private void btnCompleteOrder_Click(object sender, RoutedEventArgs e)
        {
            OrderComponent ordercomponent = Entities.OrderComponent.Find(ord.IdOrderComponent);
            ordercomponent.IdOrderStatus = 2;
            var quantityOC = Entities.OrderComponent.Where(i => i.IdOrderComponent == ordercomponent.IdOrderComponent).FirstOrDefault();

            if (Entities.SellingComponent.Where(i => i.IdComponent == ordercomponent.IdComponent).FirstOrDefault() == null)
            {
                SellingComponent sellingComponent = new SellingComponent
                {
                    IdComponent = Entities.Component.Where(i => i.IdComponent == ordercomponent.IdComponent).Select(i => i.IdComponent).FirstOrDefault(),
                    Quantity = quantityOC.Quantity,
                    SalesValue = Calculations.CalculationComponent(Entities.Component.Where(i => i.IdComponent == ordercomponent.IdComponent).Select(i => i.Price).FirstOrDefault(), quantityOC.Quantity, Entities.SupplierType.Where(i => i.IdSupplierType == ordercomponent.IdSupplierType).Select(i => i.Price).FirstOrDefault())
                };

                Entities.SellingComponent.Add(sellingComponent);
            }
            else if (Entities.SellingComponent.Where(i => i.IdComponent == ordercomponent.IdComponent).FirstOrDefault() != null)
            {
                var sellingService = Entities.SellingComponent.Where(i => i.IdComponent == ordercomponent.IdComponent).FirstOrDefault();
                sellingService.SalesValue += Calculations.CalculationComponent(Entities.Component.Where(i => i.IdComponent == ordercomponent.IdComponent).Select(i => i.Price).FirstOrDefault(), quantityOC.Quantity, Entities.SupplierType.Where(i => i.IdSupplierType == ordercomponent.IdSupplierType).Select(i => i.Price).FirstOrDefault());
                sellingService.Quantity += quantityOC.Quantity;
            }

            Entities.SaveChanges();
            tblockStatus.Text = Entities.OrderStatus.Where(i => i.IdOrderStatus == ordercomponent.IdOrderStatus).Select(i => i.OrderStatusName).FirstOrDefault();
            MessageBox.Show("Заказ успешно закрыт.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            btnConfirmOrder.IsEnabled = false;
            btnCompleteOrder.IsEnabled = false;
        }
    }
}
