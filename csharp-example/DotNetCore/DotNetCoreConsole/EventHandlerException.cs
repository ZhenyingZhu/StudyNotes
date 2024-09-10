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
    public class EventHandlerException
    {
        // https://stackoverflow.com/questions/3114543/should-event-handlers-in-c-sharp-ever-raise-exceptions
        public static void TestMain()
        {
            var c = new Counter(10);
            c.ThresholdReached += HandleThresholdReachedEvent;

            Console.WriteLine("Press a");
            while (Console.ReadKey(true).KeyChar == 'a')
            {
                Console.WriteLine("increase by 1");
                c.Add(1);
            }
        }

        private static void HandleThresholdReachedEvent(object sender, ThresholdReachedEventArgs e)
        {
            Console.WriteLine($"The threshold of {e.Threshold} was reached at {e.TimeReached}.");

            if (e.Value >= e.Threshold * 2)
            {
                Console.WriteLine($"The double of threshold is exceeded.");
                throw new ApplicationException($"The double of threshold is exceeded.");
            }
        }

        private class ThresholdReachedEventArgs : EventArgs
        {
            public int Threshold { get; set; }
            public int Value { get; set; }
            public DateTime TimeReached { get; set; }
        }

        private class Counter
        {
            private int threshold;
            private int total;

            public Counter(int threshold)
            {
                this.threshold = threshold;
            }

            public void Add(int x)
            {
                total += x;
                if (total >= threshold)
                {
                    ThresholdReachedEventArgs eventArgs = new ThresholdReachedEventArgs()
                    {
                        Threshold = threshold,
                        Value = total,
                        TimeReached = DateTime.UtcNow
                    };

                    OnThresholdReached(eventArgs);
                }
            }

            public event EventHandler<ThresholdReachedEventArgs> ThresholdReached;

            protected virtual void OnThresholdReached(ThresholdReachedEventArgs e)
            {
                this.ThresholdReached?.Invoke(this, e);
            }
        }
    }
}
