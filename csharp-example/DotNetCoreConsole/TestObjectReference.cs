using System;
using System.Collections.Generic;
using System.Linq;

namespace TestObjectReference
{
    class MyClass
    {
        public int Value
        {
            get;
            set;
        }

        public MyClass(int v)
        {
            this.Value = v;
        }
    }

    public class TestMain
    {
        public static void testMain()
        {
            MyClass mc1 = new MyClass(1);
            MyClass mc2 = mc1;
            System.Console.WriteLine("Before change mc1, value: {0}", mc1.Value);
            System.Console.WriteLine("Before change mc2, value: {0}", mc2.Value);
            mc2.Value = 2;
            System.Console.WriteLine("After change mc1, value: {0}", mc1.Value);
            System.Console.WriteLine("After change mc2, value: {0}", mc2.Value);
        }
    }
}
