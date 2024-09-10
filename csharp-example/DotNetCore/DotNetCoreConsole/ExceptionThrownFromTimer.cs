using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DotNetCoreConsole
{
    public class ExceptionThrownFromTimer
    {
        private System.Timers.Timer timer;
        private bool throwException = false;

        public static void TestMain()
        {
            Console.WriteLine("Before setup timer");
            try
            {
                ExceptionThrownFromTimer t = new ExceptionThrownFromTimer();
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
