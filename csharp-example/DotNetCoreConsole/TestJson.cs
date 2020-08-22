using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestJson
{
    public class TestMain
    {
        // Still in progress.
        private static string GetJsonProperty(string jsonStr)
        {
            return Regex.Replace(jsonStr, "[^\\w_.]+", "_", RegexOptions.Compiled);
        }

        private static void Parse()
        {
            string input = "\"Key1\":\"abc-123-d5e\",\"Key2\":0";

            MatchCollection res = Regex.Matches(input, "\"Key1\":\"([\\w|-]+)\",\"Key2\":(\\d)", RegexOptions.Singleline);
            foreach (Match m in res)
            {
                Console.WriteLine(Guid.Parse(m.Groups[1].Value));
                Console.WriteLine(m.Groups[2].Value);
            }
        }

        public static void testMain()
        {
            Parse();
        }
    }
}
