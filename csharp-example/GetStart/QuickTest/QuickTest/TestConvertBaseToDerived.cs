using System;
using System.Collections.Generic;

namespace QuickTest
{
    class TestConvertBaseToDerived
    {
        public static void TestMain()
        {
            List<MyBase> myBases = new List<MyBase>() {null, new MyDerive1()};

            foreach (var mb in myBases)
            {
                if (mb is MyDerive1)
                {
                    Console.WriteLine("mb is MyDerive1");
                }
                else
                {
                    Console.WriteLine("mb is not MyDerive1");
                }
            }
        }
    }
}
