using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestAwait
{
    public class TestMain
    {
        private async Task<int> LongRunningOperationAsync(int seconds)
        {
            System.Console.WriteLine(string.Format("LongRunningOperationAsync {0}s before", seconds));
            await Task.Delay(seconds * 1000);
            System.Console.WriteLine(string.Format("LongRunningOperationAsync {0}s after", seconds));
            return 1;
        }

        private async Task Runner()
        {
            System.Console.WriteLine("Runner start");

            Task<int> t1 = this.LongRunningOperationAsync(3);
            Task<int> t2 = this.LongRunningOperationAsync(2);
            Task<int> t3 = this.LongRunningOperationAsync(1);
            
            //t1.Wait();
            await t1;
            await t2;
            await t3;

            System.Console.WriteLine("Runner complete");
        }

        public static void testMain()
        {
            TestMain myClass = new TestMain();
            myClass.Runner().Wait();
        }
    }
}
