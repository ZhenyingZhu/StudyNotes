using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestNullOrSet
    {
        public static void TestMain()
        {
            TestNullOrSet c = new TestNullOrSet();
            string val = c.GetValueAsNull(true) ?? c.SetValue();
            Console.WriteLine(val);

            val = c.GetValueAsNull(false) ?? c.SetValue();
            Console.WriteLine(val);
        }

        private string GetValueAsNull(bool beNull)
        {
            if (beNull)
            {
                return null;
            }
            return "banana";
        }

        private string SetValue()
        {
            return "apple";
        }

    }
}
