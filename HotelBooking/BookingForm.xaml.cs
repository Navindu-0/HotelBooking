using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for BookingForm.xaml
    /// </summary>
    public partial class BookingForm : Window
    {
        public BookingForm()
        {
            InitializeComponent();
            FillGrid();
        }

        private void FillGrid()
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = @"SELECT 
                                        id AS Id,
                                        room_id AS RoomId,
                                        customer_id AS CustomerId,
                                        check_in AS CheckIn,
                                        check_out AS CheckOut,
                                        total_amount AS TotalAmount,
                                        status AS Status
                                     FROM bookings";
                    MySqlDataAdapter adapter = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    dgBookings.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading bookings: " + ex.Message);
            }
        }


        private void btnAddBooking_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = @"INSERT INTO bookings (room_id, customer_id, check_in, check_out, total_amount, status) 
                                     VALUES (@RoomId, @CustomerId, @CheckIn, @CheckOut, @TotalAmount, @Status)";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@RoomId", txtRoomId.Text);
                    cmd.Parameters.AddWithValue("@CustomerId", txtCustomerId.Text);
                    cmd.Parameters.AddWithValue("@CheckIn", dpCheckIn.SelectedDate);
                    cmd.Parameters.AddWithValue("@CheckOut", dpCheckOut.SelectedDate);
                    cmd.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                    cmd.Parameters.AddWithValue("@Status", (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Booking added successfully!");
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding booking: " + ex.Message);
            }
        }

        private void btnUpdateBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is DataRowView rowView)
            {
                int bookingId = Convert.ToInt32(rowView["Id"]);

                try
                {
                    DbConnection db = new DbConnection();
                    using (MySqlConnection con = db.ConnectDB())
                    {
                        con.Open();
                        string query = @"UPDATE bookings 
                                         SET room_id=@RoomId, customer_id=@CustomerId, check_in=@CheckIn, 
                                             check_out=@CheckOut, total_amount=@TotalAmount, status=@Status 
                                         WHERE id=@Id";
                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@RoomId", txtRoomId.Text);
                        cmd.Parameters.AddWithValue("@CustomerId", txtCustomerId.Text);
                        cmd.Parameters.AddWithValue("@CheckIn", dpCheckIn.SelectedDate);
                        cmd.Parameters.AddWithValue("@CheckOut", dpCheckOut.SelectedDate);
                        cmd.Parameters.AddWithValue("@TotalAmount", txtTotalAmount.Text);
                        cmd.Parameters.AddWithValue("@Status", (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                        cmd.Parameters.AddWithValue("@Id", bookingId);

                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Booking updated successfully!");
                        FillGrid();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating booking: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Select a booking to update.");
            }
        }

        private void btnDeleteBooking_Click(object sender, RoutedEventArgs e)
        {
            if (dgBookings.SelectedItem is DataRowView rowView)
            {
                int bookingId = Convert.ToInt32(rowView["Id"]);

                MessageBoxResult result = MessageBox.Show("Delete this booking?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DbConnection db = new DbConnection();
                        using (MySqlConnection con = db.ConnectDB())
                        {
                            con.Open();
                            string query = "DELETE FROM bookings WHERE id=@Id";
                            MySqlCommand cmd = new MySqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@Id", bookingId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Booking deleted successfully!");
                            FillGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting booking: " + ex.Message);
                    }
                }
            }
            else
            {
                MessageBox.Show("Select a booking to delete.");
            }
        }

        private void dgBookings_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgBookings.SelectedItem is DataRowView rowView)
            {
                txtRoomId.Text = rowView["RoomId"].ToString();
                txtCustomerId.Text = rowView["CustomerId"].ToString();
                dpCheckIn.SelectedDate = Convert.ToDateTime(rowView["CheckIn"]);
                dpCheckOut.SelectedDate = Convert.ToDateTime(rowView["CheckOut"]);
                txtTotalAmount.Text = rowView["TotalAmount"].ToString();
                cmbStatus.Text = rowView["Status"].ToString();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            txtCustomerId.Clear();
            txtRoomId.Clear();
            txtTotalAmount.Clear();
            dpCheckIn.SelectedDate = null;
            dpCheckOut.SelectedDate = null;
            cmbStatus.SelectedIndex = -1;
            dgBookings.SelectedIndex = -1;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }

}

