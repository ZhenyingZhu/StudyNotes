using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Timers;

namespace DotNetCoreConsole
{
    public class TestTimer
    {
        private static Timer timer;
        internal static TimeSpan? TimerInterval { get; set; }

        public static void TestMain()
        {
            SetupRefresh();


            Console.WriteLine("Type anything to end.");
            Console.ReadLine();
        }

        private static void SetupRefresh()
        {
            if (TimerInterval == null)
            {
                TimerInterval = TimeSpan.FromSeconds(3);
            }

            timer = new Timer(TimerInterval.Value.TotalMilliseconds);
            timer.Elapsed += (sender, args) => DoSomething();
            timer.Enabled = true;
        }

        private static void DoSomething()
        {
            Console.WriteLine("Doing someting");
        }
    }
}