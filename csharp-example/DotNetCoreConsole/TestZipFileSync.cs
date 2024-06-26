using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestZipFileSync
    {
        public static void TestMain()
        {
            // https://stackoverflow.com/questions/22604941/how-can-i-unzip-a-file-to-a-net-memory-stream
            try
            {
                string zipPath = Path.Combine(@"C:\git", "TestZip.zip");
                string unzipPath = Path.Combine(@"C:\git", "UnzipTestZip");
                Directory.CreateDirectory(unzipPath);

                using (FileStream zipFile = File.OpenRead(zipPath))
                using (ZipArchive zip = new ZipArchive(zipFile, ZipArchiveMode.Read))
                {
                    foreach (ZipArchiveEntry entry in zip.Entries)
                    {
                        string unzipFilePath = Path.Combine(unzipPath, entry.FullName);
                        Console.WriteLine(unzipFilePath);
                        FileInfo unzipFile = new FileInfo(unzipFilePath);
                        if (unzipFile.Extension != ".ini")
                        {
                            continue;
                        }

                        // https://stackoverflow.com/questions/2955402/how-do-i-create-directory-if-it-doesnt-exist-to-create-a-file
                        unzipFile.Directory.Create();

                        using (StreamReader sr = new StreamReader(entry.Open()))
                        using (StreamWriter sw = new StreamWriter(unzipFilePath, false))
                        {
                            sw.Write(sr.ReadToEnd());
                        }
                    }
                }

                DirectoryInfo unzipFolder = new DirectoryInfo(Path.Combine(unzipPath, "xyz"));
                foreach (FileInfo existingIniFile in unzipFolder.GetFiles("*.ini"))
                {
                    Console.WriteLine(existingIniFile.FullName);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}
