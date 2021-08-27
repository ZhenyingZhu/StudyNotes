using System;

namespace TestTimeSpan
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan span = TimeSpan.FromMinutes(1);

            Console.WriteLine("Time span {0}", span.TotalSeconds);

            string str = "Press a key to exit.";
            Console.WriteLine(str);

            Console.ReadLine();
        }
    }
}
