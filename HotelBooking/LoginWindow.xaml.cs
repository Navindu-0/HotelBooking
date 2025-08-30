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
using System.Windows.Shapes;

namespace HotelBooking
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private bool isPasswordVisible = false;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Password.Trim();

            if (username == "Navindu" && password == "1234")
            {
                MainWindow main = new MainWindow();
                main.Show();

                this.Close();
            }
            else
            {
                lblError.Text = "❌ Invalid username or password.";
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnShowPassword_Click(object sender, RoutedEventArgs e)
        {
            if (isPasswordVisible)
            {
                // Hide password
                txtPassword.Password = txtPasswordVisible.Text;
                txtPasswordVisible.Visibility = Visibility.Collapsed;
                txtPassword.Visibility = Visibility.Visible;
                btnShowPassword.Content = "👁";
                isPasswordVisible = false;
            }
            else
            {
                // Show password
                txtPasswordVisible.Text = txtPassword.Password;
                txtPasswordVisible.Visibility = Visibility.Visible;
                txtPassword.Visibility = Visibility.Collapsed;
                btnShowPassword.Content = "🙈";
                isPasswordVisible = true;
            }
        }
    }
}
