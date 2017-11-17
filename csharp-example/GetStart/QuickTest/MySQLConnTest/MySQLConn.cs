using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace MySQLConnTest
{
    public class MySQLConn
    {
        public void Connect(string uid, string pwd)
        {
            string myConnectionString = string.Format("server=localhost;database=taggedfs;uid={0};pwd={1};", uid, pwd);

            MySqlConnection conn = new MySqlConnection(myConnectionString);

            try
            {
                conn.Open();
                Console.WriteLine("Connection Open ! ");

                string query = "SELECT * FROM taggedfs.tag ORDER BY id";
                var cmd = new MySqlCommand(query, conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    string tagId = reader.GetString(0);
                    string tagName = reader.GetString(1);
                    Console.WriteLine(tagId + "," + tagName);
                }
                conn.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("Can not open connection ! ");
            }
        }

        public static void TestMain()
        {
            Console.WriteLine("User name and password:");
            string uid = Console.ReadLine();
            string pwd = Console.ReadLine();

            MySQLConn conn = new MySQLConn();

            conn.Connect(uid, pwd);
        }
    }
}
