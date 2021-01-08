using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestCreateDirectory
    {
        public static void TestMain()
        {
            Directory.CreateDirectory("TestDirectory");

            // Create it again to see if it would fail if the directory already exists.
            Directory.CreateDirectory("TestDirectory");
        }
    }
}
