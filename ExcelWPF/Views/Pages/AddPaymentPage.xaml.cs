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

namespace ExcelWPF.Views.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddPaymentPage.xaml
    /// </summary>
    public partial class AddPaymentPage : Page
    {
        int countSum = 1;
        Core db = new Core();
        List<Category> arrayCategories = new List<Category>();
        List<string> arrayCategoriesName = new List<string>();
        public AddPaymentPage()
        {
            InitializeComponent();
            CountTextBox.Text = countSum.ToString();
            arrayCategories = db.context.Category.ToList();
            foreach (Category item in arrayCategories)
            {
                arrayCategoriesName.Add(item.name_category);
            }
            CategoriesCombo.ItemsSource = arrayCategoriesName;
            CategoriesCombo.SelectedIndex = 0;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }

        private void AddCountButton_Click(object sender, RoutedEventArgs e)
        {
            countSum++;
            CountTextBox.Text = countSum.ToString();
        }

        

        private void MinusCountButton_Click(object sender, RoutedEventArgs e)
        {
            if (countSum > 1)
            {
                countSum--;
                CountTextBox.Text = countSum.ToString();
            }
        }

        private void AddPaymentButton_Click(object sender, RoutedEventArgs e)
        {
            Payment newPayment = new Payment()
            {
                date_payment = DateTime.Now,
                category_id = CategoriesCombo.SelectedIndex + 1,
                user_id = db.context.Users.FirstOrDefault(x => x.login == Properties.Settings.Default.ActiveUser).id_user,
                name = PaymentNameTextBox.Text,
                count = Convert.ToInt32(CountTextBox.Text),
                price = Convert.ToInt32(PriceTextBox.Text),
                cost = Convert.ToInt32(CountTextBox.Text) * Convert.ToInt32(PriceTextBox.Text)

            };
            db.context.Payment.Add(newPayment);
            db.context.SaveChanges();
            if (db.context.SaveChanges() == 0)
            {
                MessageBox.Show("Успешно добавлено");
            }
        }
    }
}
