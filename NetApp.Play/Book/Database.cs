using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Play.Book
{
    internal class Database
    {
        const string MySQLConnectionString = @"Server=192.168.1.108;Database=avmt;User=sqladmin;Password=123456;";
        public void CallProcedure()
        {
            using (MySqlConnection conn = new MySqlConnection(MySQLConnectionString))
            {
                using (MySqlCommand cmd = new MySqlCommand("query_classifies", conn))
                {
                    conn.Open();
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@con", "14068");
                    cmd.Parameters["@con"].Direction = System.Data.ParameterDirection.Input;

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var dir = reader.GetString(0);
                        }
                    }
                }
                using (MySqlCommand cmd = new MySqlCommand("count_classifies", conn))
                {
                    if(conn.State== System.Data.ConnectionState.Closed)
                    {
                        conn.Open();
                    }
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@param1", MySqlDbType.Int32);
                    cmd.Parameters["@param1"].Direction = System.Data.ParameterDirection.Output;

                    cmd.ExecuteNonQuery();
                    var value = cmd.Parameters["@param1"].Value;
                }
            }
        }
    }
}
