using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TeleprompterConsole
{
    // https://github.com/dotnet/docs/blob/master/samples/csharp/getting-started/console-teleprompter/
    // https://docs.microsoft.com/en-us/dotnet/articles/csharp/tutorials/console-teleprompter
    public class Program
    {
        public static void Main(string[] args)
        {
            RunTelepromoter().Wait();
        }

        private static async Task RunTelepromoter()
        {
            var config = new TelePrompterConfig();
            var displayTask = ShowTelepromoter(config);

            var speedTask = GetInput(config);
            await Task.WhenAny(displayTask, speedTask);

        }

        private static async Task GetInput(TelePrompterConfig config)
        {
            Action work = () =>
            {
                do {
                    var key = Console.ReadKey(true);
                    if (key.KeyChar == '>')
                        config.UpdateDelay(-10);
                    else if (key.KeyChar == '<')
                        config.UpdateDelay(+10);
                } while (!config.Done);
            };
            await Task.Run(work);
        }

        private static async Task ShowTelepromoter(TelePrompterConfig config)
        {
            var words = ReadFrom("sampleQuotes.txt");
            foreach (var word in words)
            {
                Console.Write(word);
                if (!string.IsNullOrEmpty(word))
                {
                    await Task.Delay(config.DelayInMilliseconds);
                }
            }
            config.SetDone();
        }

        // Enumerator method
        static IEnumerable<string> ReadFrom(string file)
        {
            string line;
            using (var reader = File.OpenText(file))
            {
                while ((line = reader.ReadLine()) != null)
                {
                    var lineLength = 0;

                    var words = line.Split(' ');
                    foreach (var word in words)
                    {
                        lineLength += word.Length + 1;
                        if (lineLength > 70)
                        {
                            lineLength = 0;
                            yield return Environment.NewLine;
                        }

                        // will it skip one word?
                        yield return word + " ";
                    }
                    yield return Environment.NewLine;
                }
            }
        }
    }
}