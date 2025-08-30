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
using static HotelBooking.CustomersView;

namespace HotelBooking
{
    /// <summary>
    /// Interaction logic for CustomersView.xaml
    /// </summary>
    public partial class CustomersView : Window
    {
        public CustomersView()
        {
            InitializeComponent();
            fillGrid();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txtName.Text))
                {
                    MessageBox.Show("Full Name is required.", "Validation Error",
                                    MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();

                    string query = @"INSERT INTO customers (full_name, phone, email, national_id) 
                             VALUES (@FullName, @Phone, @Email, @NationalID)";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@NationalID", txtNational.Text.Trim());

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer added successfully ✅", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                
                fillGrid();

                txtName.Clear();
                txtPhone.Clear();
                txtEmail.Clear();
                txtNational.Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding customer: " + ex.Message,
                                "Database Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
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
                                full_name AS FullName,
                                phone AS Phone,
                                email AS Email,
                                national_id AS NationalID
                             FROM customers";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgCustomers.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading customers: " + ex.Message,
                                "Database Error",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }

        public class Customer
        {
            public int id { get; set; }
            public string FullName { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string NationalID { get; set; }
        }


        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to delete.", "No Selection",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView row = dgCustomers.SelectedItem as DataRowView;
            if (row == null) return;

            MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this customer?",
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
                        string query = "DELETE FROM customers WHERE id=@ID";

                        using (MySqlCommand cmd = new MySqlCommand(query, con))
                        {
                            cmd.Parameters.AddWithValue("@ID", row["Id"]);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Customer deleted successfully 🗑", "Success",
                                    MessageBoxButton.OK, MessageBoxImage.Information);

                    fillGrid();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error deleting customer: " + ex.Message,
                                    "Database Error",
                                    MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgCustomers.SelectedItem == null)
            {
                MessageBox.Show("Please select a customer to update.", "No Selection",
                                MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DataRowView row = dgCustomers.SelectedItem as DataRowView;
            if (row == null) return;

            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = @"UPDATE customers 
                             SET full_name=@FullName, phone=@Phone, email=@Email, national_id=@NationalID
                             WHERE id=@ID";

                    using (MySqlCommand cmd = new MySqlCommand(query, con))
                    {
                        cmd.Parameters.AddWithValue("@FullName", txtName.Text.Trim());
                        cmd.Parameters.AddWithValue("@Phone", txtPhone.Text.Trim());
                        cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
                        cmd.Parameters.AddWithValue("@NationalID", txtNational.Text.Trim());
                        cmd.Parameters.AddWithValue("@ID", row["Id"]);

                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Customer updated successfully ✅", "Success",
                                MessageBoxButton.OK, MessageBoxImage.Information);

                fillGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating customer: " + ex.Message,
                                "Database Error",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void dgCustomers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgCustomers.SelectedItem is DataRowView rowView)
            {
                txtID.Text = rowView["Id"].ToString();
                txtName.Text = rowView["FullName"].ToString();
                txtPhone.Text = rowView["Phone"].ToString();
                txtEmail.Text = rowView["Email"].ToString();
                txtNational.Text = rowView["NationalId"].ToString();
            }
        }

        private void txtName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtEmail.Clear();
            txtID.Clear();
            txtName.Clear();
            txtPhone.Clear();
            txtNational.Clear();
            txtEmail.Clear();
            dgCustomers.UnselectAll();

        }
    }
}
