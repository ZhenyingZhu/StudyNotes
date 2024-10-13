using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConsole
{
    public class TestAsyncTasks
    {
        public static void TestMain()
        {
            var m = new TestAsyncTasks();

            try
            {
                Task.WaitAll(m.WaitAllProcess());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e);
            }
        }

        private async Task WaitAllProcess()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task t1 = this.ManagerProcess(token, source);
            Task t2 = this.ChildProcess(token);

            await t1;
            await t2;
        }

        private async Task ManagerProcess(CancellationToken token, CancellationTokenSource source)
        {
            int count = 0;
            while (count < 3)
            {
                count++;
                System.Console.WriteLine("ManagerProcess_" + count);

                await Task.Delay(TimeSpan.FromSeconds(3), token);
            }

            System.Console.WriteLine("ManagerProcess trigger cancel.");
            source.Cancel();
        }

        private async Task ChildProcess(CancellationToken token)
        {
            int count = 0;
            while (!token.IsCancellationRequested && count < 10)
            {
                count++;
                System.Console.WriteLine("ChildProcess_" + count);

                await Task.Delay(TimeSpan.FromSeconds(2), token);
            }
        }
    }
}
