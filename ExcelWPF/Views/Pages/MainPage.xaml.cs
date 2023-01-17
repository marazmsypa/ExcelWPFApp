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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Excel = Microsoft.Office.Interop.Excel;

namespace ExcelWPF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        Excel.Application application = new Excel.Application();
        Core db = new Core();
        List<Users> arrayUsers = new List<Users>();
        List<Payment> arrayPayment = new List<Payment>();
        List<Category> arrayCategory = new List<Category>();
        List<DateTime> dateTimes = new List<DateTime>() { };
        List<string> comboCategory = new List<string>();
        bool loaded = false;
        public MainPage()
        {
            InitializeComponent();
            arrayPayment = db.context.Payment.Where(x => x.user_id == db.context.Users.FirstOrDefault(y => y.login == Properties.Settings.Default.ActiveUser).id_user).ToList();
            arrayCategory = db.context.Category.ToList();
            int i = 0;
            foreach (Payment item in arrayPayment)
            {
                if (i != 0)
                {
                    DateTime itemDate = Convert.ToDateTime(item.date_payment);
                    DateTime prevItemDate = Convert.ToDateTime(arrayPayment[i - 1].date_payment);
                    if (!(itemDate.Year == prevItemDate.Year && itemDate.Month == prevItemDate.Month && itemDate.Day == prevItemDate.Day))
                    {
                        dateTimes.Add(itemDate);                      
                    }
                }
                i++;
            }
            dateTimes.Sort();
            FisrtDateCombo.ItemsSource = dateTimes;
            LastDateCombo.ItemsSource = dateTimes;
            FisrtDateCombo.SelectedIndex = 0;
            CategoryCombo.SelectedIndex = 0;
            LastDateCombo.SelectedIndex = dateTimes.Count - 1;
            MainGrid.ItemsSource = arrayPayment;
            comboCategory.Add("Все");
            foreach (Category item in arrayCategory)
            {
                comboCategory.Add(item.name_category);
            }
            CategoryCombo.ItemsSource = comboCategory;
            UpdateGrid();
            loaded = true;
        }

        private void OnchetButton_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.ActiveUser == String.Empty)
            {
                arrayUsers = db.context.Users.ToList();
            }
            else
            {
                arrayUsers = db.context.Users.Where(x => x.login == Properties.Settings.Default.ActiveUser).ToList();
            }
            application.Visible = true;
            Excel.Workbook workbook = application.Workbooks.Add();
            foreach (Users user in arrayUsers)
            {
                Excel.Worksheet worksheet = workbook.Sheets.Add();
                worksheet.Name = user.last_name + " " + user.first_name + " " + user.patronymic_name;
                worksheet.Cells[1][1] = "Дата платежа";
                worksheet.Cells[2][1] = "Название";
                worksheet.Cells[3][1] = "Стоимость";
                worksheet.Cells[4][1] = "Кол-во";
                worksheet.Cells[5][1] = "Сумма";
                int currentRow = 2;

                foreach (Category category in arrayCategory)
                {
                    List<Payment> payments = db.context.Payment.Where(x => x.category_id == category.id_category && x.user_id == user.id_user).ToList();
                    if (payments.Count > 0)
                    {
                        Excel.Range rng = worksheet.get_Range($"A{currentRow}:E{currentRow}");
                        rng.Merge();
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                        worksheet.Cells[1][currentRow] = category.name_category;
                        currentRow++;
                        foreach (Payment payment in payments)
                        {
                            worksheet.Cells[1][currentRow] = payment.date_payment.ToString();
                            worksheet.Cells[2][currentRow] = payment.name;
                            worksheet.Cells[3][currentRow] = payment.cost;
                            worksheet.Cells[4][currentRow] = payment.count;
                            worksheet.Cells[5][currentRow] = payment.cost * payment.count;
                            currentRow++;
                        }
                        rng = worksheet.get_Range($"A{currentRow}:D{currentRow}");
                        rng.Merge();
                        rng.HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                        worksheet.Cells[1][currentRow] = "ИТОГО:";
                        worksheet.Cells[5][currentRow].Formula = $"=SUM(E{currentRow - payments.Count}:E{currentRow - 1})";
                        currentRow++;
                    }
                    else
                    {
                        continue;
                    }

                }
                worksheet.Columns.AutoFit();
                Excel.Range borderRange = worksheet.get_Range($"A{1}:E{currentRow - 1}");
                borderRange.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle = Excel.XlLineStyle.xlContinuous;
                borderRange.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle = Excel.XlLineStyle.xlContinuous;
                borderRange.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle = Excel.XlLineStyle.xlContinuous;
                borderRange.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle = Excel.XlLineStyle.xlContinuous;
                borderRange.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle = Excel.XlLineStyle.xlContinuous;
                borderRange.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlContinuous;
            }
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            application.Quit();
        }

        public void UpdateGrid()
        {
           
            arrayPayment = db.context.Payment.Where(x => x.user_id == db.context.Users.FirstOrDefault(y => y.login == Properties.Settings.Default.ActiveUser).id_user).ToList();
            arrayPayment = arrayPayment.Where(x => x.date_payment >= dateTimes[FisrtDateCombo.SelectedIndex]).ToList();
            arrayPayment = arrayPayment.Where(x => x.date_payment <= dateTimes[LastDateCombo.SelectedIndex]).ToList();
            
            if (CategoryCombo.SelectedIndex != 0)
            {
                string name = comboCategory[CategoryCombo.SelectedIndex];
                int currentCategoryId = db.context.Category.FirstOrDefault(y => y.name_category == name).id_category;
                arrayPayment = arrayPayment.Where(x => x.category_id == currentCategoryId).ToList();
            }
            
            MainGrid.ItemsSource = arrayPayment;
        }

        private void FisrtDateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                UpdateGrid();
            }
            
        }

        private void LastDateCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                UpdateGrid();
            }
        }

        private void CategoryCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (loaded)
            {
                UpdateGrid();
            }
        }

        private void DiagrammButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new DiagrammsPage());
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddPaymentPage());
        }
    }
}
