using ExcelWPF.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ExcelWPF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для DiagrammsPage.xaml
    /// </summary>
    public partial class DiagrammsPage : Page
    {
        Core db = new Core();
        public DiagrammsPage()
        {
            InitializeComponent();
            ChartPayments.ChartAreas.Add(new ChartArea("Main"));

            var currentSeries = new Series("Payments")
            {
                IsValueShownAsLabel = true
            };
            ChartPayments.Series.Add(currentSeries);
            NameTextBox.Text = db.context.Users.FirstOrDefault(x => x.login == Properties.Settings.Default.ActiveUser).first_name + " " + db.context.Users.FirstOrDefault(x => x.login == Properties.Settings.Default.ActiveUser).last_name;
            
            ComboChartTypes.ItemsSource = Enum.GetValues(typeof(SeriesChartType));
        }

        private void ExportButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ComboUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboChartTypes.SelectedItem is SeriesChartType currentType)
            {
                Series currentSeries = ChartPayments.Series.FirstOrDefault();
                currentSeries.ChartType = currentType;
                currentSeries.Points.Clear();

                var categoriesList = db.context.Category.ToList();
                foreach (var category in categoriesList)
                {
                    currentSeries.Points.AddXY(category.name_category, db.context.Payment.ToList().Where(x => x.user_id == db.context.Users.FirstOrDefault(p => p.login == Properties.Settings.Default.ActiveUser).id_user && x.category_id == category.id_category).Sum(x => x.count * x.price));
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
