using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TestPrint
    {
        static void Print(string name, DateTime date)
        {
            Console.WriteLine("\n {0}, {1:d}, {1:t}", name, date);
        }

        public static void TestMain()
        {
            Console.WriteLine("Hello World!");

            Console.Write("Your name:");
            var name = Console.ReadLine();
            var date = DateTime.Now;

            Print(name, date);
        }
    }
}
