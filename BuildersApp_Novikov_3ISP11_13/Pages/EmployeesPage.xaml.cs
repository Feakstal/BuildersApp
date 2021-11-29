using BuildersApp_Novikov_3ISP11_13.Helper;
using BuildersApp_Novikov_3ISP11_13.Views;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для EmployeesPage.xaml
    /// </summary>
    public partial class EmployeesPage : Page
    {

        Entities Entities = new Entities();

        List<Employee> listEmployee = new List<Employee>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Фамилия (по возрастанию)",
            "Фамилия (по убыванию)",
            "Дата рождения (по возрастанию)",
            "Дата рождения (по убыванию)"
        };

        List<string> listFiltrPost = new List<string>();
        List<string> listFiltrIsDeleted = new List<string>();

        public EmployeesPage()
        {
            InitializeComponent();

            if (AuthWindow.Role.Equals("Менеджер"))
            {
                btnDelete.Visibility = Visibility.Collapsed;
                btnAdd.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Collapsed;
            }

            LvEmployees.ItemsSource = Entities.Employee.ToList();
            List<Post> posts = Entities.Post.ToList();
            List<Deleted> dels = Entities.Deleted.ToList();

            foreach (Post i in posts)
            {
                listFiltrPost.Add(i.PostName);
            }

            foreach (Deleted i in dels)
            {
                listFiltrIsDeleted.Add(i.DeletedName);
            }

            listFiltrPost.Insert(0, "Все категории");
            cboxFiltrPost.ItemsSource = listFiltrPost;
            cboxFiltrPost.SelectedIndex = 0;


            listFiltrIsDeleted.Insert(0, "Все категории");
            cboxFiltrIsDeleted.ItemsSource = listFiltrIsDeleted;
            cboxFiltrIsDeleted.SelectedIndex = 0;


            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listEmployee = Entities.Employee.ToList();

            int selectSort = cboxSort.SelectedIndex;
            switch (selectSort)
            {
                case 0:
                    listEmployee = listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;
                case 1:
                    listEmployee = listEmployee.OrderByDescending(i => i.IdEmployee).ToList();
                    break;
                case 2:
                    listEmployee = listEmployee.OrderBy(i => i.LastName).ToList();
                    break;
                case 3:
                    listEmployee = listEmployee.OrderByDescending(i => i.LastName).ToList();
                    break;
                case 4:
                    listEmployee = listEmployee.OrderBy(i => i.DateOfBirth).ToList();
                    break;
                case 5:
                    listEmployee = listEmployee.OrderByDescending(i => i.DateOfBirth).ToList();
                    break;
                default:
                    listEmployee = listEmployee.OrderBy(i => i.IdEmployee).ToList();
                    break;
            }

            if (cboxFiltrPost.SelectedIndex != 0)
            {
                listEmployee = listEmployee.Where(i => i.IdPost == cboxFiltrPost.SelectedIndex).ToList();
            }

            if (cboxFiltrIsDeleted.SelectedIndex != 0)
            {
                listEmployee = listEmployee.Where(i => i.IdDeleted == cboxFiltrIsDeleted.SelectedIndex).ToList();
            }

            listEmployee = listEmployee.Where(i => i.LastName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvEmployees.ItemsSource = listEmployee;
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow();
            addEmployeeWindow.ShowDialog();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (LvEmployees.SelectedItem != null)
            {
                AddEmployeeWindow addEmployeeWindow = new AddEmployeeWindow((Employee)LvEmployees.SelectedItem);
                addEmployeeWindow.ShowDialog();
            }
            else MessageBox.Show("Вы не выбрали сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (!(LvEmployees.SelectedItem is Employee employee))
            {
                switch (MessageBox.Show("Выберите сотрудника.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error))
                {
                    case MessageBoxResult.OK:
                        return;
                }
            }
            else
            {
                if (MessageBox.Show("Вы подтверждаете удаление сотрудника?", "Удаление сотрудника", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    if(employee.IdDeleted == 1)
                        MessageBox.Show("Сотрудник уже удален.", "Удаление сотрудника", MessageBoxButton.OK, MessageBoxImage.Error);
                    else
                    {
                        employee.IdDeleted = 1;
                        Entities.SaveChanges();
                        MessageBox.Show("Сотрудник успешно удалён.", "Удаление сотрудника", MessageBoxButton.OK, MessageBoxImage.Information);
                        LvEmployees.ItemsSource = Entities.Employee.ToList();
                        LvEmployees.Items.Refresh();
                    }
                }
            }
        }
        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrPost_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void cboxFiltrIsDeleted_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LvEmployees.Items.Refresh();
            LvEmployees.ItemsSource = Entities.Employee.ToList();
        }
    }
}
