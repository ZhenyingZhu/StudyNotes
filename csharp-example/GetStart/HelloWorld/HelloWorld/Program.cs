using System;

namespace HelloWorld
{
    class Program
    {
        static void Print(string name, DateTime date)
        {
            Console.WriteLine("\n {0}, {1:d}, {1:t}", name, date);
        }

        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            Console.Write("Your name:");
            var name = Console.ReadLine();
            var date = DateTime.Now;

            Print(name, date);

            Console.WriteLine("Press key");
            Console.ReadKey();
        }
    }
}