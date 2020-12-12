using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;

namespace DotNetCoreConsole
{
    public class TestAssemblyTraverse
    {
        public static void TestMain()
        {
            //Assembly curr = typeof(DotNetCoreConsole.TestAssemblyTraverse).Assembly;
            Assembly curr = Assembly.LoadFrom("DotNetCoreConsole.dll");
            Type[] types = curr.GetTypes();
            foreach (Type t in types)
            {
                Console.WriteLine(t.FullName);
            }
        }
    }
}