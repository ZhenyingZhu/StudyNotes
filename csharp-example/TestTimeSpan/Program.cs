using System;

namespace TestTimeSpan
{
    class Program
    {
        static void Main(string[] args)
        {
            TimeSpan span = TimeSpan.FromMinutes(1);

            Console.WriteLine("Time span {0}", span.TotalSeconds);
        }
    }
}
