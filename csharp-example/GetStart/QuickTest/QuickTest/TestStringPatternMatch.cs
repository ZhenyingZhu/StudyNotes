using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuickTest
{
    class TestStringPatternMatch
    {
        private static void ValidateStringPatterns(string str, Regex pattern)
        {
            Match match = pattern.Match(str);
            if (!match.Success)
            {
                string message = string.Format(
                    "Value '{0}' does not match the valid pattern of '{1}'.",
                    str,
                    pattern);
                Console.WriteLine(message);
            }
            else
            {
                Console.WriteLine("Success.");
            }
        }

        public static void TestMain()
        {
            Regex pattern = new Regex(@"^(([sS][mM][tT][pP]\:((([\u0021-\u007E-[<>\(\)\[\]\\\.,;:@]]|(\\[\u0000-\u007F]))+(\.([\u0021-\u007E-[<>\(\)\[\]\\\.,;:@]]|(\\[\u0000-\u007F]))+)*))@[^@]*))$");
            ValidateStringPatterns(
                "SMTP:abc@efg.com",
                pattern);
        }
    }
}
