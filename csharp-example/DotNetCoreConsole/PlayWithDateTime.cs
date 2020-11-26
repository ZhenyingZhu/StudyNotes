using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class PlayWithDateTime
    {
        public static void TestMain()
        {
            DateTime date = DateTime.UtcNow;

            System.Console.WriteLine(date);
            System.Console.WriteLine(date.ToString("o"));
        }
    }
}
