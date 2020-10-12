using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace DotNetCoreConsole
{
    public class TestMethodAugRef
    {
        public static void testMain()
        {
            int[] arr = new int[10];
            arr[0] = 1;
            TestMethodAugRef cur = new TestMethodAugRef();
            cur.TestArgRef(arr);

            System.Console.WriteLine("Start print");
            foreach(int i in arr)
            {
                System.Console.WriteLine(i);
            }
        }

        private void TestArgRef(int[] testArg = null)
        {
            // if change the var address, then the outer arg is changed.
            // testArg = new int[10];
            // but if not, then it works.
            if (testArg != null)
            {
                testArg[0] = 2;
            }
        }
    }
}
