using BuildersApp_Novikov_3ISP11_13.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для SellingComponentsPage.xaml
    /// </summary>
    public partial class SellingComponentsPage : Page
    {

        Entities Entities = new Entities();

        List<SellingComponent> listSellingComponent = new List<SellingComponent>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Товар (по возрастанию)",
            "Товар (по убыванию)",
            "Количество (по возрастанию)",
            "Количество (по убыванию)",
            "Объем продаж (по возрастанию)",
            "Объем продаж (по убыванию)"
        };

        public SellingComponentsPage()
        {
            InitializeComponent();
            LvSC.ItemsSource = Entities.SellingComponent.ToList();

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listSellingComponent = Entities.SellingComponent.ToList();

            switch (cboxSort.SelectedIndex)
            {
                case 0:
                    listSellingComponent = listSellingComponent.OrderBy(i => i.IdSellingComponent).ToList();
                    break;
                case 1:
                    listSellingComponent = listSellingComponent.OrderByDescending(i => i.IdSellingComponent).ToList();
                    break;
                case 2:
                    listSellingComponent = listSellingComponent.OrderBy(i => i.IdComponent).ToList();
                    break;
                case 3:
                    listSellingComponent = listSellingComponent.OrderByDescending(i => i.IdComponent).ToList();
                    break;
                case 4:
                    listSellingComponent = listSellingComponent.OrderBy(i => i.Quantity).ToList();
                    break;
                case 5:
                    listSellingComponent = listSellingComponent.OrderByDescending(i => i.Quantity).ToList();
                    break;
                case 6:
                    listSellingComponent = listSellingComponent.OrderBy(i => i.SalesValue).ToList();
                    break;
                case 7:
                    listSellingComponent = listSellingComponent.OrderByDescending(i => i.SalesValue).ToList();
                    break;
                default:
                    listSellingComponent = listSellingComponent.OrderBy(i => i.IdSellingComponent).ToList();
                    break;
            }

            listSellingComponent = listSellingComponent.Where(i => i.Component.ComponentName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvSC.ItemsSource = listSellingComponent;
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Filtr();
        }

        private void cboxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filtr();
        }
    }
}
