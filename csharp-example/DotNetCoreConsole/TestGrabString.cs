using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestGrabString
    {
        private enum Food
        {
            Pie
        }

        public static void TestMain()
        {
            string a = "ApplePie";
            string pie = Enum.GetName(typeof(Food), Food.Pie);
            string res = a.Substring(0, a.Length - pie.Length);
            Console.WriteLine(res + "|");
        }
    }
}