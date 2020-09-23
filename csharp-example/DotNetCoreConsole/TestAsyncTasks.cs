using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TestAsyncTasks
{
    public class TestMain
    {
        public static void testMain()
        {
            var m = new TestMain();

            try
            {
                Task.WaitAll(m.WaitAllProcess());
            }
            catch (AggregateException e)
            {
                Console.WriteLine(e);
            }
        }

        public async Task WaitAllProcess()
        {
            CancellationTokenSource source = new CancellationTokenSource();
            CancellationToken token = source.Token;

            Task t1 = this.ManagerProcess(token, source);
            Task t2 = this.ChildProcess(token);

            await t1;
            await t2;
        }

        public async Task ManagerProcess(CancellationToken token, CancellationTokenSource source)
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

        public async Task ChildProcess(CancellationToken token)
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
