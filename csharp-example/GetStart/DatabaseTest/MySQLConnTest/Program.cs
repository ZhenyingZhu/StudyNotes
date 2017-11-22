using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MySQLConnTest
{
    class Program
    {
        static void Main(string[] args)
        {
            MySQLConn.TestMain();

            Console.WriteLine("Press key");
            Console.ReadKey();
        }
    }
}
