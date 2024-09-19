using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestNullCheck2
    {
        private class TestNull
        {
            public int? val;
        }

        public static void TestMain()
        {
            TestNull t1 = new TestNull {val = 1};
            TestNull t2 = new TestNull();
            TestNull t3 = null;

            Console.WriteLine(NullString(t1?.val));
            Console.WriteLine(NullString(t2?.val));
            Console.WriteLine(NullString(t3?.val));
        }

        private static string NullString(int? val)
        {
            if (val == null)
            {
                return "null";
            }
            return val.Value.ToString();
        }
    }

}
