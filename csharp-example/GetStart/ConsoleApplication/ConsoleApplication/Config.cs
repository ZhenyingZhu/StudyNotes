using static System.Math;

namespace TeleprompterConsole
{
    internal class TelePrompterConfig
    {
        private object lockHandle = new object();
        public int DelayInMilliseconds { get; private set; } = 200;

        public void UpdateDelay(int increment)
        {
            var newDelay = Min(DelayInMilliseconds + increment, 1000);
            newDelay = Max(20, newDelay);

            lock (lockHandle)
            {
                DelayInMilliseconds = newDelay;
            }
        }

        public bool Done => done;

        private bool done;

        public void SetDone()
        {
            done = true;
        }
    }
}
