using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;

namespace HotelBooking
{
    public partial class PaymentsView : Window
    {
        public PaymentsView()
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
                        id AS PaymentId,
                        booking_id AS BookingId,
                        amount AS Amount,
                        payment_date AS PaymentDate,
                        method AS Method,
                        status AS Status
                        FROM payments";

                    MySqlCommand cmd = new MySqlCommand(query, con);
                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    dgPayments.ItemsSource = dt.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading payments: " + ex.Message);
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = "INSERT INTO payments (booking_id, amount, payment_date, method, status) VALUES (@BookingId, @Amount, @PaymentDate, @Method, @Status)";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@BookingId", txtBookingId.Text);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@PaymentDate", dpPaymentDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@Method", (cmbMethod.SelectedItem as ComboBoxItem)?.Content.ToString());
                    cmd.Parameters.AddWithValue("@Status", (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Payment added successfully!");
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding payment: " + ex.Message);
            }
        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPaymentId.Text))
                {
                    MessageBox.Show("Please select a payment to update.");
                    return;
                }

                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = "UPDATE payments SET booking_id=@BookingId, amount=@Amount, payment_date=@PaymentDate, method=@Method, status=@Status WHERE id=@Id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", txtPaymentId.Text);
                    cmd.Parameters.AddWithValue("@BookingId", txtBookingId.Text);
                    cmd.Parameters.AddWithValue("@Amount", txtAmount.Text);
                    cmd.Parameters.AddWithValue("@PaymentDate", dpPaymentDate.SelectedDate);
                    cmd.Parameters.AddWithValue("@Method", (cmbMethod.SelectedItem as ComboBoxItem)?.Content.ToString());
                    cmd.Parameters.AddWithValue("@Status", (cmbStatus.SelectedItem as ComboBoxItem)?.Content.ToString());
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Payment updated successfully!");
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating payment: " + ex.Message);
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtPaymentId.Text))
                {
                    MessageBox.Show("Please select a payment to delete.");
                    return;
                }

                DbConnection db = new DbConnection();
                using (MySqlConnection con = db.ConnectDB())
                {
                    con.Open();
                    string query = "DELETE FROM payments WHERE id=@Id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", txtPaymentId.Text);
                    cmd.ExecuteNonQuery();

                    MessageBox.Show("Payment deleted successfully!");
                    FillGrid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting payment: " + ex.Message);
            }
        }

        private void BtnClear_Click(object sender, RoutedEventArgs e)
        {
            txtPaymentId.Clear();
            txtBookingId.Clear();
            txtAmount.Clear();
            dpPaymentDate.SelectedDate = null;
            cmbMethod.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void dgPayments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPayments.SelectedItem is DataRowView rowView)
            {
                txtPaymentId.Text = rowView["PaymentId"].ToString();
                txtBookingId.Text = rowView["BookingId"].ToString();
                txtAmount.Text = rowView["Amount"].ToString();
                dpPaymentDate.SelectedDate = Convert.ToDateTime(rowView["PaymentDate"]);
                cmbMethod.Text = rowView["Method"].ToString();
                cmbStatus.Text = rowView["Status"].ToString();
            }
        }
    }
}
