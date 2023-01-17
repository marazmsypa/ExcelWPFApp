using ExcelWPF.Classes;
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
    /// Логика взаимодействия для AuthPage.xaml
    /// </summary>
    public partial class AuthPage : Page
    {
        Core db = new Core();
        PasswordCheck passwordCheck = new PasswordCheck();
        public AuthPage()
        {
            InitializeComponent();
            List<Users> users = db.context.Users.ToList();
            List<string> logins = new List<string>() { };
            foreach (Users item in users)
            {
                logins.Add(item.login);
            }
            UserComboBox.ItemsSource = logins;
            UserComboBox.SelectedIndex = 0;
            Properties.Settings.Default.ActiveUser = UserComboBox.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (!passwordCheck.RightPassword(UserPasswordBox.Password))
            {
                MessageBox.Show("Вы ввели неподходящий пароль");
            }
            else
            {
                
                if (UserPasswordBox.Password == db.context.Users.FirstOrDefault(x => x.login == UserComboBox.SelectedValue.ToString()).password)
                {
                    this.NavigationService.Navigate(new MainPage());
                   
                }
                else
                {
                    MessageBox.Show("Вы неправильно ввели пароль");
                }
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void UserComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Properties.Settings.Default.ActiveUser = UserComboBox.SelectedValue.ToString();
            Properties.Settings.Default.Save();
        }
    }
}
