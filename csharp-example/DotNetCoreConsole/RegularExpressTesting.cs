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

                // Console.WriteLine(GetField(input, field));
                Console.WriteLine(Replace(input));
            }
        }

        private static string GetField(string input, string field)
        {
            string regex = $"{field}:([\\w|\\d|-]+),";
            MatchCollection m = Regex.Matches(input, regex);
            return m[0].Groups[1].Value;
        }

        private static string Replace(string input)
        {
            const string PRIMARY_KEY_PATTERN = @"\/PrimaryKey [0-9a-zA-Z+\/=]+";
            const string SECONDARY_KEY_PATTERN = @"\/SecondaryKey [0-9a-zA-Z+\/=]+";

            input = Regex.Replace(input, PRIMARY_KEY_PATTERN, "<scrubbed primary key>");
            input = Regex.Replace(input, SECONDARY_KEY_PATTERN, "<scrubbed secondary key>");

            return input;
        }
    }
}
