using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class ReturnRepeatEntries
    {
        public static void TestMain()
        {
            List<MyClass> myList = new List<MyClass>();
            for (int i = 0; i < 3; i++)
            {
                MyClass mci = new MyClass
                {
                    num = i,
                    shouldBeSelected = true
                };
                myList.Add(mci);
            }

            MyClass mc = new MyClass
            {
                num = 2,
                shouldBeSelected = true
            };
            myList.Add(mc);

            var res = myList.Where(x => x.shouldBeSelected).Select(x => x.num); //.ToList();
            System.Console.WriteLine("[" + String.Join(", ", res) + "]");
        }

        private class MyClass
        {
            public int num
            {
                set;
                get;
            }

            public bool shouldBeSelected
            {
                set;
                get;
            }
        }
    }
}
