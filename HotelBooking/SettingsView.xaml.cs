using System;
using System.Data;
using System.Windows;
using MySql.Data.MySqlClient;

namespace HotelBooking
{
    public partial class SettingsView : Window
    {
        public SettingsView()
        {
            InitializeComponent();
            LoadSettings();
        }

        private void LoadSettings()
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = "SELECT * FROM settings LIMIT 1";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataReader reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        txtHotelName.Text = reader["hotel_name"].ToString();
                        txtAddress.Text = reader["address"].ToString();
                        txtPhone.Text = reader["phone"].ToString();
                        txtEmail.Text = reader["email"].ToString();
                        cmbTheme.Text = reader["theme_color"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading settings: " + ex.Message);
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();

                    string queryCheck = "SELECT COUNT(*) FROM settings";
                    MySqlCommand cmdCheck = new MySqlCommand(queryCheck, con);
                    int count = Convert.ToInt32(cmdCheck.ExecuteScalar());

                    string query;
                    if (count > 0)
                    {
                        // Update settings
                        query = @"UPDATE settings 
                                  SET hotel_name=@HotelName, address=@Address, phone=@Phone, email=@Email, theme_color=@Theme";
                    }
                    else
                    {
                        // Insert settings
                        query = @"INSERT INTO settings (hotel_name, address, phone, email, theme_color) 
                                  VALUES (@HotelName, @Address, @Phone, @Email, @Theme)";
                    }

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@HotelName", txtHotelName.Text);
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                    cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                    cmd.Parameters.AddWithValue("@Theme", (cmbTheme.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString());

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Settings saved successfully!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error saving settings: " + ex.Message);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtHotelName.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtEmail.Clear();
            cmbTheme.SelectedIndex = 0;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
