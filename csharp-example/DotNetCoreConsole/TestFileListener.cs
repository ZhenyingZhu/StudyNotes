using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestFileListener
    {
        private const int RetryCount = 3;

        public static void testMain()
        {
            string fileName = "TestFile.txt";
            using (FileSystemWatcher watcher = new FileSystemWatcher(".", fileName))
            {
                // Watch for changes in LastAccess and LastWrite times, and
                // the renaming of files or directories.
                // The CreationTime can monitor file replacement. See File.Replace(st, st, null)
                watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.CreationTime;

                // Add event handlers.
                watcher.Changed += OnChanged;

                // Begin watching.
                watcher.EnableRaisingEvents = true;

                // Wait for the user to quit the program.
                Console.WriteLine("Press 'q' to quit the sample.");
                while (Console.Read() != 'q') ;
            }
        }

        // Define the event handlers.
        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            // Specify what is done when a file is changed, created, or deleted.
            Console.WriteLine($"File: {e.FullPath} {e.ChangeType}");

            for (int i = 0; i < RetryCount; i++)
            {
                try
                {
                    Console.WriteLine(File.ReadAllText(e.FullPath));
                    break;
                }
                catch (IOException ioEx)
                {
                    Console.WriteLine(ioEx);
                }
            }
            
        }
    }
}
