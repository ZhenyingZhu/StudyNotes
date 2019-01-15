using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace TestJson
{
    public class TestMain
    {
        // Still in progress.
        private static string GetJsonProperty(string jsonStr)
        {
            return Regex.Replace(jsonStr, "[^\\w_.]+", "_", RegexOptions.Compiled);
        }

        public static void testMain()
        {
            System.Console.WriteLine();
        }
    }
}
