using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestValuePassing
    {
        public static void TestMain()
        {
            Guid val;
            Guid newVal = Guid.NewGuid();
            val = val = newVal;
            System.Console.WriteLine("Guid val:" + val);
        }
    }
}
