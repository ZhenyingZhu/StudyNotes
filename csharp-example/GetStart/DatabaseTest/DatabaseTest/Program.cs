using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySQLConnTest;
using SQLiteTest;

namespace DatabaseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            //MySQLConn.TestMain();
            SQLiteConn.TestMain();

            Console.WriteLine("Press key");
            Console.ReadKey();
        }
    }
}
