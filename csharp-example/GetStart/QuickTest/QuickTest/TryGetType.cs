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
                var type_ass = GetType().Assembly;
                Console.WriteLine(type_ass.ToString());
                var sys_ass = System.Reflection.Assembly.GetExecutingAssembly();
                Console.WriteLine(type_ass.ToString());
                Console.WriteLine(type_ass == sys_ass); // true
            }
        }

        public static void TestMain()
        {
            TestClass tc = new TestClass();
            tc.GetTypeName();
        }
    }
}
