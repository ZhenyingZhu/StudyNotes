using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TestValuePassing
{
    public class TestMain
    {
        public static void testMain()
        {
            Guid val;
            Guid newVal = Guid.NewGuid();
            val = val = newVal;
            System.Console.WriteLine("Guid val:" + val);
        }
    }
}