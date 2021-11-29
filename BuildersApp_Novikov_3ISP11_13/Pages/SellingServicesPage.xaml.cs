using BuildersApp_Novikov_3ISP11_13.Helper;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace BuildersApp_Novikov_3ISP11_13.Pages
{
    /// <summary>
    /// Логика взаимодействия для SellingServicesPage.xaml
    /// </summary>
    public partial class SellingServicesPage : Page
    {

        Entities Entities = new Entities();

        List<SellingService> listSellingService = new List<SellingService>();

        List<string> listSort = new List<string>()
        {
            "Номер (по возрастанию)",
            "Номер (по убыванию)",
            "Услуга (по возрастанию)",
            "Услуга (по убыванию)",
            "Количество (по возрастанию)",
            "Количество (по убыванию)",
            "Объем продаж (по возрастанию)",
            "Объем продаж (по убыванию)"
        };

        public SellingServicesPage()
        {
            InitializeComponent();
            LvSC.ItemsSource = Entities.SellingService.ToList();

            cboxSort.ItemsSource = listSort;
            cboxSort.SelectedIndex = 0;
        }

        void Filtr()
        {
            listSellingService = Entities.SellingService.ToList();

            switch (cboxSort.SelectedIndex)
            {
                case 0:
                    listSellingService = listSellingService.OrderBy(i => i.IdSellingService).ToList();
                    break;
                case 1:
                    listSellingService = listSellingService.OrderByDescending(i => i.IdSellingService).ToList();
                    break;
                case 2:
                    listSellingService = listSellingService.OrderBy(i => i.IdService).ToList();
                    break;
                case 3:
                    listSellingService = listSellingService.OrderByDescending(i => i.IdService).ToList();
                    break;
                case 4:
                    listSellingService = listSellingService.OrderBy(i => i.Quantity).ToList();
                    break;
                case 5:
                    listSellingService = listSellingService.OrderByDescending(i => i.Quantity).ToList();
                    break;
                case 6:
                    listSellingService = listSellingService.OrderBy(i => i.SalesValue).ToList();
                    break;
                case 7:
                    listSellingService = listSellingService.OrderByDescending(i => i.SalesValue).ToList();
                    break;
                default:
                    listSellingService = listSellingService.OrderBy(i => i.IdSellingService).ToList();
                    break;
            }

            listSellingService = listSellingService.Where(i => i.Service.ServiceName.ToLower().Contains(tboxSearch.Text.ToLower())).ToList();
            LvSC.ItemsSource = listSellingService;
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
