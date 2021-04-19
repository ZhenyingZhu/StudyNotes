using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;
using System.Threading;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;


namespace DotNetCoreConsole
{
    public class TestTimer
    {
        private Timer timer;
        private readonly TimeSpan timerInterval = TimeSpan.FromSeconds(3);

        private int counter = 0;

        public static void TestMain()
        {
            TestTimer t = new TestTimer();
            t.SetupRefresh();

            Console.WriteLine("Type anything to end.");
            var str = Console.ReadLine();

            // t.timer.Enabled = false;
            Console.WriteLine($"exit {str}");
        }

        private void SetupRefresh()
        {
            timer = new Timer(this.timerInterval.TotalMilliseconds);
            timer.Elapsed += (sender, args) => this.DoSomething().Wait();
            timer.Enabled = true;
        }

        private async Task DoSomething()
        {
            Console.WriteLine($"Start do someting {this.counter++}. {DateTime.Now}");
            await Task.Delay(2000);
            Console.WriteLine($"Done someting {this.counter}. {DateTime.Now}");
        }
    }
}