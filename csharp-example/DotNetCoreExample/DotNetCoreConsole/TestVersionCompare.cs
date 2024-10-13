using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestVersionCompare
    {
        public static void TestMain()
        {
            Version a = Version.Parse("1.0.0.0");
            Version b = Version.Parse("1.0.0.1");

            Console.WriteLine(a < b);
            Console.WriteLine(a == b);
            Console.WriteLine(Version.Equals(a, null));
        }
    }
}
