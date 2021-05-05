using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestRegexScrubber
    {
        private const char Separator = '_';

        // All the chars other than \w or _.
        private static readonly Regex KeyScrubber = new Regex($"[^a-zA-Z0-9{Separator}]+", RegexOptions.Compiled);

        public static void TestMain()
        {
            string part1 = "___apple_1.0";
            string part2 = "pie";

            // Replace all special chars with _ and remove prefix _.
            part1 = KeyScrubber.Replace(part1, Separator.ToString()).Trim(Separator);
            part2 = KeyScrubber.Replace(part2, Separator.ToString()).Trim(Separator);

            string res = string.Join(Separator.ToString(), part1, part2, nameof(Version)).ToLower();

            Console.WriteLine(res);
        }
    }
}