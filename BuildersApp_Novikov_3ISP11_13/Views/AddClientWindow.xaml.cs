using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BuildersApp_Novikov_3ISP11_13.Class;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddClientWindow.xaml
    /// </summary>
    public partial class AddClientWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditClient = false;
        private Client EditClient = new Client();

        public AddClientWindow()
        {
            InitializeComponent();
            cboxGender.ItemsSource = Entities.Gender.Select(i => i.GenderName).ToList();
            winAddClient = this;
        }

        public AddClientWindow(Client client)
        {
            InitializeComponent();
            cboxGender.ItemsSource = Entities.Gender.Select(i => i.GenderName).ToList();
            winAddClient = this;
            if (client != null)
            {
                EditClient = client;
                CheckEditClient = true;
                tboxLastName.Text = client.LastName;
                tboxFirstName.Text = client.FirstName;
                tboxFatherName.Text = client.FatherName;
                tboxEmail.Text = client.Email;
                tboxPhone.Text = client.Phone;
                dpBirthday.Text = client.DateOfBirth.ToString();
                tboxAddress.Text = client.Address;
                cboxGender.SelectedItem = Entities.Gender.Where(i => i.IdGender == client.IdGender).Select(i => i.GenderName).FirstOrDefault();
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winAddClient.DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditClient)
            {
                Client client = Entities.Client.Find(EditClient.IdClient);
                if (tboxLastName.Text.Length == 0 || tboxFirstName.Text.Length == 0 || cboxGender.SelectedItem == null || tboxAddress.Text.Length == 0 || dpBirthday.SelectedDate == null || tboxPhone.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные клиента?", "Обновлениие данных клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    client.LastName = tboxLastName.Text;
                    client.FirstName = tboxFirstName.Text;
                    client.FatherName = tboxFatherName.Text;
                    client.Phone = tboxPhone.Text;
                    client.Email = tboxEmail.Text;
                    client.Address = tboxAddress.Text;
                    client.IdGender = Entities.Gender.Where(i => i.GenderName == cboxGender.SelectedItem.ToString()).Select(i => i.IdGender).FirstOrDefault();
                    client.DateOfBirth = Convert.ToDateTime(dpBirthday.SelectedDate);

                    bool result = ValidatorExtensions.IsValidEmailAddress(tboxEmail.Text);
                    if (!result)
                        MessageBox.Show("Электронная почта не соответствует маске.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxPhone.Text.Length != 11)
                        MessageBox.Show("Телефон не содержит 11 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (Calculations.AgeLessThan18(DateTime.Now, client.DateOfBirth) || client.DateOfBirth > DateTime.Now)
                        MessageBox.Show("Данному сотруднику меньше 18-ти лет или он еще не родился.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxLastName.Text.Length > 75 | tboxFirstName.Text.Length > 75 || tboxFatherName.Text.Length > 75 || tboxEmail.Text.Length > 45 || tboxAddress.Text.Length > 125)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.SaveChanges();
                        MessageBox.Show("Данные клиента успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        Close();
                    }
                }
            }
            if (!CheckEditClient)
            {
                if (tboxLastName.Text.Length == 0 || tboxFirstName.Text.Length == 0 || cboxGender.SelectedItem == null || tboxAddress.Text.Length == 0 || dpBirthday.SelectedDate == null || tboxPhone.Text.Length == 0)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить клиента?", "Добавление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Client client = new Client
                    {
                        LastName = tboxLastName.Text,
                        FirstName = tboxFirstName.Text,
                        FatherName = tboxFatherName.Text,
                        Phone = tboxPhone.Text,
                        Email = tboxEmail.Text,
                        Address = tboxAddress.Text,
                        DateOfBirth = Convert.ToDateTime(dpBirthday.SelectedDate),
                        IdGender = Entities.Gender.Where(i => i.GenderName == cboxGender.SelectedItem.ToString()).Select(i => i.IdGender).FirstOrDefault(),
                        IdDeleted = 2
                    };

                    bool result = ValidatorExtensions.IsValidEmailAddress(tboxEmail.Text);
                    if (!result)
                        MessageBox.Show("Электронная почта не соответствует маске.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxPhone.Text.Length != 11)
                        MessageBox.Show("Телефон не содержит 11 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (Calculations.AgeLessThan18(DateTime.Now, client.DateOfBirth) || client.DateOfBirth > DateTime.Now)
                        MessageBox.Show("Данному сотруднику меньше 18-ти лет или он еще не родился.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxLastName.Text.Length > 75 | tboxFirstName.Text.Length > 75 || tboxFatherName.Text.Length > 75 || tboxEmail.Text.Length > 45 || tboxAddress.Text.Length > 125)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.Client.Add(client);
                        Entities.SaveChanges();
                        MessageBox.Show($"Клиент {tboxLastName.Text} {tboxFirstName.Text} успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        this.Close();
                    }
                }
            }
        }

        private void tboxPhone_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlLetters(sender, e);
        }

        private void tboxLastName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void tboxFirstName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void tboxFatherName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void tboxEmail_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlNumbers(sender, e);
        }

        private void tboxAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }
    }
}
