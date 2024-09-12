using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreConsole
{
    public class MultiThreading
    {
        private System.Timers.Timer timer;
        private bool throwException = false;

        public static async Task<T> UnwrapTaskWithTimeout<T>(Task<Task<T>> task, TimeSpan? timeout)
        {
            if (timeout != null)
            {
                Task timeoutTask = Task.Delay(timeout.Value);

                if (await Task.WhenAny(task, timeoutTask) != timeoutTask)
                {
                    // Outer task has finished before timeout. Use await to unwrap its result or exception.
                    Task<T> innerTask = await task;

                    if (await Task.WhenAny(innerTask, timeoutTask) != timeoutTask)
                    {
                        T result = await innerTask;
                        return result;
                    }
                }

                string message = "Task timed out";

                throw new TimeoutException(message);
            }
            else
            {
                return await await task;
            }
        }

        public static void ExceptionThrownFromTimer()
        {
            Console.WriteLine("Before setup timer");
            try
            {
                MultiThreading t = new MultiThreading();
                Task.WaitAll(t.ThrowExInTimer());
            }
            catch (System.Exception)
            {
                Console.WriteLine("Exception caught in the main thread");
                throw;
            }
        }

        private async Task ThrowExInTimer()
        {
            this.timer = new System.Timers.Timer(10 * 1000);
            this.timer.Elapsed += (sender, args) =>
            {
                Console.WriteLine("Timer fires");
                this.throwException = true;

                // throw new ApplicationException("throw an exception");
            };

            this.timer.Enabled = true;

            await Task.Delay(new TimeSpan(0, 0, 15));
            if (this.throwException == true)
            {
                throw new ApplicationException("throw an exception");
            }

            this.timer.Enabled = false;
        }
    }
}
