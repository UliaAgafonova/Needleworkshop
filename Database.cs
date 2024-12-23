using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace NeedleWorkShop2
{
    internal class Database
    {
        MySqlConnection con = new MySqlConnection
            (@"server=sql7.freemysqlhosting.net;port=3306;database=sql7747866;user=sql7747866;password=Q5cp9ShRdy;");
        public void OpenConnection()
        {
            if (con.State == System.Data.ConnectionState.Closed)
                con.Open();
        }
        public void CloseConnection()
        {
            if (con.State == System.Data.ConnectionState.Open)
                con.Close();
        }
        public MySqlConnection GetConnection()
        {
            return con;
        }
    }
}