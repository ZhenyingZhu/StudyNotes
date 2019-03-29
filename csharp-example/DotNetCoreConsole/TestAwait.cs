using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TestAwait
{
    public class TestMain
    {
        public async Task MyMethodAsync()
        {
            Task<int> longRunningTask = LongRunningOperationAsync();
            int result = await longRunningTask;

            System.Console.WriteLine(result);
        }

        private async Task<int> LongRunningOperationAsync()
        {
            await Task.Delay(1000);
            return 1;
        }

        public static void TestMain.testMain()
        {
            Task myMethod = MyMethodAsync();

            System.Console.WriteLine();
        }
    }
}
