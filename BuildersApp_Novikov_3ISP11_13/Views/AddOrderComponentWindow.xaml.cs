using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddOrderWindow.xaml
    /// </summary>
    public partial class AddOrderComponentWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditOrderComponent = false;
        private OrderComponent EditOrderComponent = new OrderComponent();
        public AddOrderComponentWindow()
        {
            InitializeComponent();
            winAddEditOrderComponent = this;

            cboxPayment.ItemsSource = Entities.Payment.Select(i => i.PaymentName).ToList();
            cboxEmployee.ItemsSource = Entities.Employee.Select(i => i.LastName).ToList();
            cboxLastName.ItemsSource = Entities.Client.Select(i => i.LastName).ToList();
            cboxComponent.ItemsSource = Entities.Component.Select(i => i.ComponentName).ToList();
        }

        public AddOrderComponentWindow(OrderComponent orderComponent)
        {
            if (orderComponent != null)
            {
                InitializeComponent();
                winAddEditOrderComponent = this;

                cboxPayment.ItemsSource = Entities.Payment.Select(i => i.PaymentName).ToList();
                cboxEmployee.ItemsSource = Entities.Employee.Select(i => i.LastName).ToList();
                cboxLastName.ItemsSource = Entities.Client.Select(i => i.LastName).ToList();
                cboxComponent.ItemsSource = Entities.Component.Select(i => i.ComponentName).ToList();

                EditOrderComponent = orderComponent;
                CheckEditOrderComponent = true;

                cboxComponent.Text = Entities.Component.Where(i => i.IdComponent == orderComponent.IdComponent).Select(i => i.ComponentName).FirstOrDefault();
                cboxLastName.Text = Entities.Client.Where(i => i.IdClient == orderComponent.IdClient).Select(i => i.LastName).FirstOrDefault();
                cboxPayment.SelectedItem = Entities.Payment.Where(i => i.IdPayment == orderComponent.IdPayment).Select(i => i.PaymentName).FirstOrDefault();
                cboxEmployee.SelectedItem = Entities.Employee.Where(i => i.IdEmployee == orderComponent.IdEmployee).Select(i => i.LastName).FirstOrDefault();
                tboxQuantity.Text = orderComponent.Quantity.ToString();
                dtpPerformanceDate.Text = orderComponent.PerformanceDate.ToString();

                if (orderComponent.IdOrderStatus == 4 || orderComponent.IdOrderStatus == 2 || orderComponent.IdDeleted == 1)
                    btnSave.IsEnabled = false;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditOrderComponent)
            {
                OrderComponent orderComponent = Entities.OrderComponent.Find(EditOrderComponent.IdOrderComponent);
                var findEmployee = Entities.Employee.Where(i => i.IdEmployee == orderComponent.IdEmployee).FirstOrDefault();
                var findPost = Entities.Post.Where(i => i.IdPost == findEmployee.IdPost).FirstOrDefault();
                
                if (cboxLastName.SelectedItem == null || cboxPayment.SelectedItem == null || cboxComponent.SelectedItem == null || cboxEmployee.SelectedItem == null || dtpPerformanceDate.Text == null || dtpPerformanceDate.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные заказа?", "Обновлениие данных заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    orderComponent.IdClient = Entities.Client.Where(i => i.LastName == cboxLastName.SelectedItem.ToString()).Select(i => i.IdClient).FirstOrDefault();
                    orderComponent.IdComponent = Entities.Component.Where(i => i.ComponentName == cboxComponent.SelectedItem.ToString()).Select(i => i.IdComponent).FirstOrDefault();
                    orderComponent.IdPayment = Entities.Payment.Where(i => i.PaymentName == cboxPayment.SelectedItem.ToString()).Select(i => i.IdPayment).FirstOrDefault();
                    orderComponent.IdEmployee = Entities.Employee.Where(i => i.LastName == cboxEmployee.SelectedItem.ToString()).Select(i => i.IdEmployee).FirstOrDefault();
                    orderComponent.PerformanceDate = Convert.ToDateTime(dtpPerformanceDate.Text);
                    orderComponent.Quantity = Convert.ToInt32(tboxQuantity.Text);

                    var findPost1 = Entities.Employee.Where(i => i.IdEmployee == orderComponent.IdEmployee).FirstOrDefault().IdPost;

                    if (Convert.ToDateTime(dtpPerformanceDate.Text) < DateTime.Now)
                        MessageBox.Show("Дата доставки заказа не может быть раньше даты создания заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (findPost1 != 4)
                        MessageBox.Show("Данный сотрудник не является курьером.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (cboxLastName.Text.Length > 75 || cboxComponent.Text.Length > 155 || cboxPayment.Text.Length > 95 || cboxEmployee.Text.Length > 75)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные заказа успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            else if (!CheckEditOrderComponent)
            {
                if (cboxLastName.SelectedItem == null || cboxPayment.SelectedItem == null || cboxComponent.SelectedItem == null || dtpPerformanceDate.Text == null || dtpPerformanceDate.Text.Length == 0 || cboxEmployee.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить заказ?", "Добавление заказа", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    OrderComponent AddOrderComponent = new OrderComponent
                    {
                        IdClient = Entities.Client.Where(i => i.LastName == cboxLastName.SelectedItem.ToString()).Select(i => i.IdClient).FirstOrDefault(),
                        IdComponent = Entities.Component.Where(i => i.ComponentName == cboxComponent.SelectedItem.ToString()).Select(i => i.IdComponent).FirstOrDefault(),
                        IdPayment = Entities.Payment.Where(i => i.PaymentName == cboxPayment.SelectedItem.ToString()).Select(i => i.IdPayment).FirstOrDefault(),
                        IdEmployee = Entities.Employee.Where(i => i.LastName == cboxEmployee.SelectedItem.ToString()).Select(i => i.IdEmployee).FirstOrDefault(),
                        IdSupplierType = 2,
                        PerformanceDate = Convert.ToDateTime(dtpPerformanceDate.Text),
                        Quantity = Convert.ToInt32(tboxQuantity.Text),
                        CreateDate = DateTime.Now,
                        IdDeleted = 2,
                        IdOrderStatus = 3
                    };

                    var findPost2 = Entities.Employee.Where(i => i.IdEmployee == AddOrderComponent.IdEmployee).FirstOrDefault().IdPost;
                        
                    if (Convert.ToDateTime(dtpPerformanceDate.Text) < DateTime.Now)
                        MessageBox.Show("Дата доставки заказа не может быть раньше даты создания заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (AddOrderComponent.Quantity > 999)
                        MessageBox.Show("Ограничение на количество товаров за 1 заказ.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (findPost2 != 4)
                        MessageBox.Show("Данный сотрудник не является курьером.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (cboxLastName.Text.Length > 75 || cboxComponent.Text.Length > 155 || cboxPayment.Text.Length > 95 || cboxEmployee.Text.Length > 75)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.OrderComponent.Add(AddOrderComponent);
                        Entities.SaveChanges();
                        MessageBox.Show("Заказ успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
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
                winAddEditOrderComponent.DragMove();
        }

        private void cboxClient_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void cboxEmployee_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void cboxComponent_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }

        private void dtpPerformanceDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlForDate(sender, e);
        }

        private void tboxQuantity_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlLetters(sender, e);
        }
    }
}
