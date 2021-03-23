using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestPathJoinNull
    {
        public static void TestMain()
        {
            try
            {
                string path = System.IO.Path.Combine(null, null, "folder");
                Console.WriteLine(path);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
