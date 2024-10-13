using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestUnzipFile
    {
        public static void TestMain()
        {
            string sourceFolder = @"TestDirectory";
            string zipFile = @"result.zip";
            string targetFolder = @"TargetDirectory";

            ZipFile.CreateFromDirectory(sourceFolder, zipFile);

            ZipFile.ExtractToDirectory(zipFile, targetFolder);
        }
    }
}
