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
    public class TestWaitAllException
    {
        public static void TestMain()
        {
            TestWaitAllException t = new TestWaitAllException();
            List<Task> tasks = new List<Task>();
            tasks.Add(t.AsyncTask(1));
            tasks.Add(t.AsyncTask(2));
            tasks.Add(t.AsyncTask(3));

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch (ApplicationException e)
            {
                // Since the exception is AggregationException, it is not caught here.
                Console.WriteLine("Exception thrown:");
                Console.WriteLine(e);
            }

            Console.WriteLine("Program complete successfully");
        }

        private async Task AsyncTask(int tid)
        {
            System.Console.WriteLine($"Task {tid} starts");

            await Task.Delay(TimeSpan.FromSeconds(1));

            if (tid == 2)
            {
                System.Console.WriteLine($"Task {tid} crashes");
                throw new ApplicationException($"Task {tid} crashes");
            }
            else
            {
                System.Console.WriteLine($"Task {tid} ends");
            }
        }
    }
}
