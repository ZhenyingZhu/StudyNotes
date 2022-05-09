using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class RegularExpressTesting
    {
        public static void TestMain()
        {
            Console.WriteLine("Type input and field");
            string input = Console.ReadLine();

            string field = string.Empty;
            while (field != "exit")
            {
                field = Console.ReadLine();

                Console.WriteLine(GetField(input, field));
            }
        }

        private static string GetField(string input, string field)
        {
            string regex = $"{field}:([\\w|\\d|-]+),";
            MatchCollection m = Regex.Matches(input, regex);
            return m[0].Groups[1].Value;
        }
    }
}
