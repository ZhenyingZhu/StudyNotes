using System;
using System.Collections.Generic;
using System.Linq;

namespace TestEqualsForStringAndEnum
{
    public enum MyEnum
    {
        StateOne = 1,

        StateTwo = 2
    }

    public class TestMain
    {
        public static void testMain()
        {
            string a = "test";
            string b = "test";

            if (a == b)
            {
                System.Console.WriteLine("== works.");
            }
            else
            {
                System.Console.WriteLine("== not works.");
            }

            MyEnum c = MyEnum.StateOne;
            MyEnum d = MyEnum.StateOne;

            System.Console.WriteLine(c);
            if (c == d)
            {
                System.Console.WriteLine("== works.");
            }
            else
            {
                System.Console.WriteLine("== not works.");
            }

        }
    }
}
