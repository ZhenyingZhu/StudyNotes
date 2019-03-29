using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestAwait
{
    public class TestMain
    {
        private async Task<int> LongRunningOperationAsync()
        {
            System.Console.WriteLine("LongRunningOperationAsync before");
            await Task.Delay(1000);
            System.Console.WriteLine("LongRunningOperationAsync after");
            return 1;
        }

        public static void testMain()
        {
            TestMain myClass = new TestMain();
            System.Console.WriteLine("testMain complete");
            myClass.LongRunningOperationAsync().Wait();

            System.Console.WriteLine("testMain complete");
        }
    }
}
