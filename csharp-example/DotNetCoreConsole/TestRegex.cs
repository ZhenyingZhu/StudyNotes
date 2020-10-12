using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestRegex
    {
        public static void testMain()
        {

            string input = "[{\"Guid\":\"b05e124f-c7cc-45a0-a6aa-8cf78c946968\"},{\"Guid\":\"b05e124f-c7cc-45a0-a6aa-8cf78c946968\"}]";
            string[] output = GetAllJSONProperty(input);

            System.Console.WriteLine(string.Join(",", output));
        }

        private static string ReplaceSpecialCharacters(string str)
        {
            // \w is all words 0-9a-zA-Z. [^] means not match any in it.
            return Regex.Replace(str, "[^\\w_.]+", "_", RegexOptions.Compiled);

            //string res = ReplaceSpecialCharacters(@"1!@#$%^&*()-_=+;:'[{]}\\|,<.>/?aA");
        }

        private static string[] GetAllJSONProperty(string str)
        {
            MatchCollection res = Regex.Matches(str, "\"Guid\":\"([\\w|-]+)\"", RegexOptions.Singleline);
            return res.Select(x => x.Groups[1].Value).ToArray();
        }
    }
}
