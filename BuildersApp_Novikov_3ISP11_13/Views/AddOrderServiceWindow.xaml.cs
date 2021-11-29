using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOrderServiceWindow.xaml
    /// </summary>
    public partial class AddOrderServiceWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditOrderService = false;
        private OrderService EditOrderService = new OrderService();

        public AddOrderServiceWindow()
        {
            InitializeComponent();
            winAddEditOrderService = this;

            cboxPayment.ItemsSource = Entities.Payment.Select(i => i.PaymentName).ToList();
            cboxEmployee.ItemsSource = Entities.Employee.Select(i => i.LastName).ToList();
            cboxLastName.ItemsSource = Entities.Client.Select(i => i.LastName).ToList();
            cboxService.ItemsSource = Entities.Service.Select(i => i.ServiceName).ToList();
        }

        public AddOrderServiceWindow(OrderService orderService)
        {
            if (orderService != null)
            {
                InitializeComponent();
                winAddEditOrderService = this;

                cboxPayment.ItemsSource = Entities.Payment.Select(i => i.PaymentName).ToList();
                cboxEmployee.ItemsSource = Entities.Employee.Select(i => i.LastName).ToList();
                cboxLastName.ItemsSource = Entities.Client.Select(i => i.LastName).ToList();
                cboxService.ItemsSource = Entities.Service.Select(i => i.ServiceName).ToList();

                EditOrderService = orderService;
                CheckEditOrderService = true;

                cboxService.Text = Entities.Service.Where(i => i.IdService == orderService.IdService).Select(i => i.ServiceName).FirstOrDefault();
                cboxLastName.Text = Entities.Client.Where(i => i.IdClient == orderService.IdClient).Select(i => i.LastName).FirstOrDefault();
                cboxPayment.SelectedItem = Entities.Payment.Where(i => i.IdPayment == orderService.IdPayment).Select(i => i.PaymentName).FirstOrDefault();
                cboxEmployee.SelectedItem = Entities.Employee.Where(i => i.IdEmployee == orderService.IdEmployee).Select(i => i.LastName).FirstOrDefault();
                dtpPerformanceDate.Text = orderService.PerformanceDate.ToString();

                if (orderService.IdOrderStatus == 4 || orderService.IdOrderStatus == 2 || orderService.IdDeleted == 1)
                    btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditOrderService)
            {
                OrderService orderService = Entities.OrderService.Find(EditOrderService.IdOrderService);

                if (cboxLastName.SelectedItem == null || cboxPayment.SelectedItem == null || cboxEmployee.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные заказа?", "Обновлениие данных заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    orderService.IdClient = Entities.Client.Where(i => i.LastName == cboxLastName.SelectedItem.ToString()).Select(i => i.IdClient).FirstOrDefault();
                    orderService.IdService = Entities.Service.Where(i => i.ServiceName == cboxService.SelectedItem.ToString()).Select(i => i.IdService).FirstOrDefault();
                    orderService.IdPayment = Entities.Payment.Where(i => i.PaymentName == cboxPayment.SelectedItem.ToString()).Select(i => i.IdPayment).FirstOrDefault();
                    orderService.IdEmployee = Entities.Employee.Where(i => i.LastName == cboxEmployee.SelectedItem.ToString()).Select(i => i.IdEmployee).FirstOrDefault();
                    orderService.PerformanceDate = Convert.ToDateTime(dtpPerformanceDate.Text);

                    var findPost1 = Entities.Employee.Where(i => i.IdEmployee == orderService.IdEmployee).FirstOrDefault().IdPost;

                    if (Convert.ToDateTime(dtpPerformanceDate.Text) < DateTime.Now)
                        MessageBox.Show("Дата выполнения услуг не может быть раньше даты создания заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (cboxLastName.Text.Length > 75 || cboxService.Text.Length > 155 || cboxPayment.Text.Length > 95 || cboxEmployee.Text.Length > 75)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (findPost1 != 3)
                        MessageBox.Show("Данный сотрудник не является сантехником.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные заказа успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            else if (!CheckEditOrderService)
            {
                if (cboxLastName.SelectedItem == null || cboxService.SelectedItem == null || cboxPayment.SelectedItem == null || dtpPerformanceDate.Text == null || dtpPerformanceDate.Text.Length == 0 || cboxEmployee.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить заказ?", "Добавление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    OrderService orderServ = new OrderService
                    {
                        IdClient = Entities.Client.Where(i => i.LastName == cboxLastName.SelectedItem.ToString()).Select(i => i.IdClient).FirstOrDefault(),
                        IdService = Entities.Service.Where(i => i.ServiceName == cboxService.SelectedItem.ToString()).Select(i => i.IdService).FirstOrDefault(),
                        IdPayment = Entities.Payment.Where(i => i.PaymentName == cboxPayment.SelectedItem.ToString()).Select(i => i.IdPayment).FirstOrDefault(),
                        IdEmployee = Entities.Employee.Where(i => i.LastName == cboxEmployee.SelectedItem.ToString()).Select(i => i.IdEmployee).FirstOrDefault(),
                        PerformanceDate = Convert.ToDateTime(dtpPerformanceDate.Text),
                        CreateDate = DateTime.Now,
                        IdDeleted = 2,
                        IdOrderStatus = 3
                    };
                    //Сделать объем продаж
                    //Попробовать решить проблему с удалением клиента/сотрудника, если заказы отсутствуют у клиент/сотрудника

                    var findPost2 = Entities.Employee.Where(i => i.IdEmployee == orderServ.IdEmployee).FirstOrDefault().IdPost;

                    if (Convert.ToDateTime(dtpPerformanceDate.Text) < DateTime.Now)
                        MessageBox.Show("Дата выполнения услуг не может быть раньше даты создания заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (cboxLastName.Text.Length > 75 || cboxService.Text.Length > 145 || cboxPayment.Text.Length > 95 || cboxEmployee.Text.Length > 75)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if(findPost2 != 3)
                        MessageBox.Show("Данный сотрудник не является сантехником.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.OrderService.Add(orderServ);
                        Entities.SaveChanges();
                        MessageBox.Show("Заказ успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winAddEditOrderService.DragMove();
        }

        private void cboxClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void cboxEmployee_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void cboxService_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }

        private void dtpPerformanceDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlForDate(sender, e);
        }
    }
}
