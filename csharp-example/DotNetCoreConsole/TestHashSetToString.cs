using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestHashSetToString
    {
        public static void TestMain()
        {
            HashSet<int> values = new HashSet<int>{1, 2, 3};
            Console.WriteLine(string.Join(",", values));
        }
    }
}
