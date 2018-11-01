using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestRegex
{
    public class TestMain
    {
        private static string ReplaceSpecialCharacters(string str)
        {
            // \w is all words 0-9a-zA-Z. [^] means not match any in it.
            return Regex.Replace(str, "[^\\w_.]+", "_", RegexOptions.Compiled);
        }

        public static void testMain()
        {
            string res = ReplaceSpecialCharacters(@"1!@#$%^&*()-_=+;:'[{]}\\|,<.>/?aA");

            System.Console.WriteLine(res);
        }
    }
}
