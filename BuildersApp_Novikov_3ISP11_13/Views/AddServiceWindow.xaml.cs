using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddServiceWindow.xaml
    /// </summary>
    public partial class AddServiceWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditService = false;
        private Service EditService = new Service();

        public AddServiceWindow()
        {
            InitializeComponent();
            winAddService = this;
        }

        public AddServiceWindow(Service service)
        {
            if (service != null)
            {
                InitializeComponent();
                winAddService = this;
                CheckEditService = true;
                EditService = service;
                tboxServiceName.Text = service.ServiceName;
                tboxPrice.Text = Convert.ToString(service.Price);
                tboxDescription.Text = service.Description;
            }
        }

        private void tboxServiceName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }

        private void tboxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlForPrice(sender, e);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditService)
            {
                Service service = Entities.Service.Find(EditService.IdService);
                if (tboxServiceName.Text.Length == 0 || tboxPrice.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные товара?", "Обновлениие данных клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    service.ServiceName = tboxServiceName.Text;
                    service.Price = Convert.ToDecimal(tboxPrice.Text);
                    service.Description = tboxDescription.Text;

                    if (tboxServiceName.Text.Length > 145 || tboxPrice.Text.Length > 30)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные товара успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            if (!CheckEditService)
            {
                if (tboxServiceName.Text.Length == 0 || tboxPrice.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить товар?", "Добавление товар", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Entities.Service.Add(new Service
                    {
                        ServiceName = tboxServiceName.Text,
                        Price = Convert.ToDecimal(tboxPrice.Text),
                        Description = tboxDescription.Text,
                        IdDeleted = 2
                    });

                    if (tboxServiceName.Text.Length > 145 || tboxPrice.Text.Length > 30)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show($"Услуга {tboxServiceName.Text} успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winAddService.DragMove();
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
