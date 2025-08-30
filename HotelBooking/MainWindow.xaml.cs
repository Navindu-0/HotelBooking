using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace HotelBooking
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnCustomers_Click_1(object sender, RoutedEventArgs e)
        {

            CustomersView newForm = new CustomersView();

            newForm.Show();
        }

        private void BtnRooms_Click(object sender, RoutedEventArgs e)
        {
            RoomsView newForm = new RoomsView();

            newForm.Show();
        }

        private void BtnBookings_Click(object sender, RoutedEventArgs e)
        {
            BookingForm newForm = new BookingForm();

            newForm.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            RoomsView newForm = new RoomsView();

            newForm.Show();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

            CustomersView newForm = new CustomersView();

            newForm.Show();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {

            BookingForm newForm = new BookingForm();

            newForm.Show();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            PaymentsView newForm = new PaymentsView();
            newForm.Show();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            SettingsView newForm = new SettingsView();
            newForm.Show();
        }
    }
}