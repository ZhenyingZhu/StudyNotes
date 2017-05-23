using System;
using System.IO;
using System.Collections.Generic;

namespace TeleprompterConsole
{
    // https://github.com/dotnet/docs/blob/master/samples/csharp/getting-started/console-teleprompter/
    // https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/console-teleprompter
    public class Program
    {
        public static void Main(string[] args)
        {
            var lines = ReadFrom("sampleQuotes.txt");
            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }

            Console.ReadKey();
        }

        // Enumerator method
        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    yield return line;
                }
            }
        }
    }
}