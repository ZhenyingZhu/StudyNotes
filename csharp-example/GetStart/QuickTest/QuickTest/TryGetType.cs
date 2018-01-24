using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    public class TryGetType
    {
        private class TestClass
        {
            public void GetTypeName()
            {
                Type t = GetType();
                Console.WriteLine(t.ToString());
            }
        }

        public static void TestMain()
        {
            TestClass tc = new TestClass();
            tc.GetTypeName();
        }
    }
}
