using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TryProcess
    {
        public static void TestMain()
        {
            string path = "C:\\windows\\system32\\notepad.exe";
            var startInfo = new ProcessStartInfo(path)
            {
                Arguments =
                    "test.txt",
                UseShellExecute = false
            };

            Process process = Process.Start(startInfo);
            if (process != null)
            {
                process.WaitForExit(1000);
                if (process.HasExited)
                {
                    Console.WriteLine(process.ExitCode);
                }
                else
                {
                    Console.WriteLine("Not exit.");
                }
            }
            else
            {
                Console.WriteLine("Process is null. Something Wrong.");
            }
        }
    }
}
