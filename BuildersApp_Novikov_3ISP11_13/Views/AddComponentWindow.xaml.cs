using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddComponentWindow.xaml
    /// </summary>
    public partial class AddComponentWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditComponent = false;
        private Component EditComponent = new Component();

        public AddComponentWindow()
        {
            InitializeComponent();
            winAddComponent = this;
        }

        public AddComponentWindow(Component component)
        {
            if(component != null)
            {
                InitializeComponent();
                winAddComponent = this;
                CheckEditComponent = true;
                EditComponent = component;
                tboxComponentName.Text = component.ComponentName;
                tboxPrice.Text = Convert.ToString(component.Price);
                tboxDescription.Text = component.Description;
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winAddComponent.DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditComponent)
            {
                Component component = Entities.Component.Find(EditComponent.IdComponent);
                if (tboxComponentName.Text.Length == 0 || tboxPrice.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные товара?", "Обновлениие данных клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    component.ComponentName = tboxComponentName.Text;
                    component.Price = Convert.ToDecimal(tboxPrice.Text);
                    component.Description = tboxDescription.Text;

                    if (tboxComponentName.Text.Length > 155 || tboxPrice.Text.Length > 30)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные товара успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            if (!CheckEditComponent)
            {
                if (tboxComponentName.Text.Length == 0 || tboxPrice.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить товар?", "Добавление товар", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Entities.Component.Add(new Component
                    {
                        ComponentName = tboxComponentName.Text,
                        Price = Convert.ToDecimal(tboxPrice.Text),
                        Description = tboxDescription.Text,
                        IdDeleted = 2
                    });

                    if (tboxComponentName.Text.Length > 155 || tboxPrice.Text.Length > 30)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show($"Клиент {tboxComponentName.Text} успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
        }

        private void tboxComponentName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }

        private void tboxPrice_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlForPrice(sender, e);
        }
    }
}
