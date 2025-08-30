using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelBooking
{
    internal class DbConnection
    {
        public MySqlConnection ConnectDB()
        {
            string con_string = "server=localhost; userid=root; pwd=; database=hotel_db";
            MySqlConnection con = new MySqlConnection(con_string);
            return con;
        }
    }
}
