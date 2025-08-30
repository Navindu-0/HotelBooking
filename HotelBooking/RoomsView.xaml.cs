using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
using static Google.Protobuf.Reflection.FieldOptions.Types;
using static HotelBooking.CustomersView;

namespace HotelBooking
{
    /// <summary>
    /// Interaction logic for RoomsView.xaml
    /// </summary>
    public partial class RoomsView : Window
    {
        public RoomsView()
        {
            InitializeComponent();
            fillGrid();
        }

        public void fillGrid()
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = @"SELECT 
                    id AS Id,
                    number AS Number,
                    type AS Type,
                    price_per_night AS PricePerNight,
                    CASE WHEN is_available = 1 THEN 'Yes'
                        ELSE 'No'
                    END AS IsAvailable
                FROM rooms";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgRooms.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading rooms: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public class Room
        {
            public string RoomID { get; set; }
            public string RoomNumber { get; set; }
            public string Type { get; set; }
            public decimal Price { get; set; }
            public string Available { get; set; }
        }


        private void btnAddRoom_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = "INSERT INTO rooms ( number, type, price_per_night , is_available ) VALUES (@RoomNumber, @RoomType, @Price, @Availability)";
                    MySqlCommand cmd = new MySqlCommand(query, con);

                    cmd.Parameters.AddWithValue("@RoomNumber", txtRoomNumber.Text);
                    cmd.Parameters.AddWithValue("@RoomType", txtType.Text);
                    cmd.Parameters.AddWithValue("@Price", txtPrice.Text);


                    bool availability = chkAvailable.IsChecked == true;
                    cmd.Parameters.AddWithValue("@Availability", availability ? "Yes" : "No");

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Room added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    fillGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdateRoom_Click(object sender, RoutedEventArgs e)
        {

            if (dgRooms.SelectedItem is DataRowView rowView)
            {
                int roomId = Convert.ToInt32(rowView["Id"]);

                string roomNumber = txtRoomNumber.Text.Trim();
                string roomType = txtType.Text.Trim();
                decimal price;
                bool priceValid = decimal.TryParse(txtPrice.Text, out price);
                bool isAvailable = chkAvailable.IsChecked == true;



                if (string.IsNullOrEmpty(roomNumber) || string.IsNullOrEmpty(roomType) || !priceValid)
                {
                    MessageBox.Show("Please fill all fields correctly.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                try
                {
                    DbConnection db = new DbConnection();
                    using (MySqlConnection con = db.ConnectDB())
                    {
                        con.Open();
                        string query = @"UPDATE rooms 
                                 SET number = @Number, 
                                     type = @Type, 
                                     price_per_night = @Price, 
                                     is_available = @Available 
                                 WHERE id = @ID";

                        MySqlCommand cmd = new MySqlCommand(query, con);
                        cmd.Parameters.AddWithValue("@Number", roomNumber);
                        cmd.Parameters.AddWithValue("@Type", roomType);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@Available", isAvailable ? 1 : 0);
                        cmd.Parameters.AddWithValue("@ID", roomId);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Room updated successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        fillGrid(); 
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating room: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Please select a room to update.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void btnDeleteRoom_Click(object sender, RoutedEventArgs e)
        {
            if (dgRooms.SelectedItem is DataRowView rowView)
            {
                int roomId = Convert.ToInt32(rowView["Id"]);

                MessageBoxResult result = MessageBox.Show(
                    "Are you sure you want to delete this room?",
                    "Confirm Deletion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Warning);

                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        DbConnection db = new DbConnection();
                        using (MySqlConnection con = db.ConnectDB())
                        {
                            con.Open();
                            string query = "DELETE FROM rooms WHERE id = @ID";
                            MySqlCommand cmd = new MySqlCommand(query, con);
                            cmd.Parameters.AddWithValue("@ID", roomId);
                            cmd.ExecuteNonQuery();

                            MessageBox.Show("Room deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            fillGrid();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting room: " + ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Please select a room to delete.", "No Selection", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void dgRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgRooms.SelectedItem is DataRowView rowView)
            {
                
                txtRoomNumber.Text = rowView["Number"].ToString();
                txtType.Text = rowView["Type"].ToString();
                txtPrice.Text = rowView["PricePerNight"].ToString();
                chkAvailable.IsChecked = (rowView["IsAvailable"].ToString() == "Yes");
            }
        }


        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtRoomNumber.Clear();
            txtType.SelectedIndex = -1;
            txtPrice.Clear();
            chkAvailable.IsChecked = false;
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}



