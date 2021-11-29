using BuildersApp_Novikov_3ISP11_13.Class;
using BuildersApp_Novikov_3ISP11_13.Helper;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace BuildersApp_Novikov_3ISP11_13.Views
{
    /// <summary>
    /// Логика взаимодействия для AddEmployeeWindow.xaml
    /// </summary>
    public partial class AddEmployeeWindow : Window
    {

        Entities Entities = new Entities();
        private bool CheckEditEmployee = false;
        private Employee EditEmployee = new Employee();

        public AddEmployeeWindow()
        {
            InitializeComponent();
            cboxGender.ItemsSource = Entities.Gender.Select(i => i.GenderName).ToList();
            cboxPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();

            winAddEmployee = this;
        }

        public AddEmployeeWindow(Employee employee)
        {

            if (employee != null)
            {
                InitializeComponent();
                cboxGender.ItemsSource = Entities.Gender.Select(i => i.GenderName).ToList();
                cboxPost.ItemsSource = Entities.Post.Select(i => i.PostName).ToList();

                winAddEmployee = this;

                EditEmployee = employee;
                CheckEditEmployee = true;
                tboxLastName.Text = employee.LastName;
                tboxFirstName.Text = employee.FirstName;
                tboxFatherName.Text = employee.FatherName;
                tboxEmail.Text = employee.Email;
                tboxPhone.Text = employee.Phone;
                tboxAddress.Text = employee.Address;
                dpBirthday.Text = employee.DateOfBirth.ToString();
                cboxPost.Text = Entities.Post.Where(i => i.IdPost == employee.IdPost).Select(i => i.PostName).FirstOrDefault();
                cboxGender.SelectedItem = Entities.Gender.Where(i => i.IdGender == employee.IdGender).Select(i => i.GenderName).FirstOrDefault();
            }
        }


        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Drag(object sender, MouseButtonEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed)
                winAddEmployee.DragMove();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (CheckEditEmployee)
            {
                Employee employee = Entities.Employee.Find(EditEmployee.IdEmployee);
                if (tboxLastName.Text.Length == 0 || tboxFirstName.Text.Length == 0 || tboxAddress.Text.Length == 0 || dpBirthday.SelectedDate == null || cboxGender.SelectedItem == null || cboxPost.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите обновить данные клиента?", "Обновлениие данных клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    employee.LastName = tboxLastName.Text;
                    employee.FirstName = tboxFirstName.Text;
                    employee.FatherName = tboxFatherName.Text;
                    employee.Phone = tboxPhone.Text;
                    employee.Email = tboxEmail.Text;
                    employee.Address = tboxAddress.Text;
                    employee.IdGender = Entities.Gender.Where(i => i.GenderName == cboxGender.SelectedItem.ToString()).Select(i => i.IdGender).FirstOrDefault();
                    employee.IdPost = Entities.Post.Where(i => i.PostName == cboxPost.SelectedItem.ToString()).Select(i => i.IdPost).FirstOrDefault();
                    employee.DateOfBirth = Convert.ToDateTime(dpBirthday.SelectedDate);

                    bool result = ValidatorExtensions.IsValidEmailAddress(tboxEmail.Text);
                    if (!result)
                        MessageBox.Show("Электронная почта не соответствует маске.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxPhone.Text.Length != 11)
                        MessageBox.Show("Телефон не содержит 11 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (Calculations.AgeLessThan18(DateTime.Now, employee.DateOfBirth) || employee.DateOfBirth > DateTime.Now)
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
            if (!CheckEditEmployee)
            {
                if (tboxLastName.Text.Length == 0 || tboxFirstName.Text.Length == 0 || tboxAddress.Text.Length == 0 || dpBirthday.SelectedDate == null || cboxGender.SelectedItem == null || cboxPost.SelectedItem == null)
                {
                    MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else if (MessageBox.Show("Вы действительно хотите добавить клиента?", "Добавление клиента", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Employee employee = new Employee
                    {
                        LastName = tboxLastName.Text,
                        FirstName = tboxFirstName.Text,
                        FatherName = tboxFatherName.Text,
                        Phone = tboxPhone.Text,
                        Email = tboxEmail.Text,
                        Address = tboxAddress.Text,
                        DateOfBirth = Convert.ToDateTime(dpBirthday.SelectedDate),
                        IdPost = Entities.Post.Where(i => i.PostName == cboxPost.SelectedItem.ToString()).Select(i => i.IdPost).FirstOrDefault(),
                        IdGender = Entities.Gender.Where(i => i.GenderName == cboxGender.SelectedItem.ToString()).Select(i => i.IdGender).FirstOrDefault(),
                        IdDeleted = 2
                    };

                    bool result = ValidatorExtensions.IsValidEmailAddress(tboxEmail.Text);
                    if (!result)
                        MessageBox.Show("Электронная почта не соответствует маске.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxPhone.Text.Length != 11)
                        MessageBox.Show("Телефон не содержит 11 символов.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (Calculations.AgeLessThan18(DateTime.Now, employee.DateOfBirth) || employee.DateOfBirth > DateTime.Now)
                        MessageBox.Show("Данному сотруднику меньше 18-ти лет или он еще не родился.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else if (tboxLastName.Text.Length > 75 | tboxFirstName.Text.Length > 75 || tboxFatherName.Text.Length > 75 || tboxEmail.Text.Length > 45 || tboxAddress.Text.Length > 125)
                        MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        Entities.Employee.Add(employee);
                        Entities.SaveChanges();
                        MessageBox.Show($"Сотрудник {tboxLastName.Text} {tboxFirstName.Text} успешно добавлен.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void tboxAddress_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInputControlSpec(sender, e);
        }
    }
}
