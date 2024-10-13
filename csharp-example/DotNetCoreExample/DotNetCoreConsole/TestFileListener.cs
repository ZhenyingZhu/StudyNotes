using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestFileListener
    {
        private const int RetryCount = 3;

        // private static bool ShouldStop = false;

        public static void TestMain()
        {
            string fileName = "TestFile.txt";
            string tempFile = fileName + ".tmp";
            using (FileSystemWatcher watcher = new FileSystemWatcher(".", fileName))
            {
                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                // The CreationTime can monitor file replacement. See File.Replace(st, st, null)
                watcher.NotifyFilter =
                    NotifyFilters.LastWrite |
                    NotifyFilters.CreationTime //|
                    //NotifyFilters.FileName |
                    //NotifyFilters.LastAccess
                    ;

                // Add event handlers.
                watcher.Changed += OnChanged;
                // watcher.Created += OnChanged;
                // watcher.Deleted += OnChanged;
                watcher.Renamed += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Replace the file
                Console.WriteLine("======= Create temp file =======");
                File.WriteAllText(tempFile, "Test2");

                Console.WriteLine("======= Replace temp file =======");
                File.Replace(tempFile, fileName, null);

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath}; ChangeType: {e.ChangeType}");

            if (e is RenamedEventArgs)
            {
                RenamedEventArgs re = e as RenamedEventArgs;
                Console.WriteLine($"--Rename: {re.OldName} {re.Name}");
            }

            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    Console.WriteLine(File.ReadAllText(e.FullPath));
                    break;
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine("========");
                    Console.WriteLine(ioEx.Message);
                    Console.WriteLine("========");

                    Thread.Sleep(10);
                }
            }
            
        }
    }
}
