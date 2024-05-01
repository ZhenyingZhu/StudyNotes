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
    public class ServiceFabricVersionUpgrade
    {
        public static void TestMain()
        {
            Uri connectionString = new Uri(@"\\127.0.0.1\c$\Users\myUser\Downloads");
            string applicationName = "MyApp";
            string applicationManifestFileName = "ApplicationManifest.xml";
            string serviceManifestFileName = "ServiceManifest.xml";
            string applicationTypeVersion = "1.0.16219.0";

            string appFolder = Path.Combine(
                connectionString.LocalPath, applicationName);
            string appManifestFilePath = Path.Combine(appFolder, applicationManifestFileName);

            string appManifestFileContent = File.ReadAllText(appManifestFilePath);

            string appVersionPattern = "ApplicationTypeVersion=\"(\\d+.\\d+.\\d+.\\d+)\"";
            Match match = Regex.Match(appManifestFileContent, appVersionPattern);
            string upgradeVersion = match.Groups[1].Value;
            if (upgradeVersion == applicationTypeVersion)
            {
                Version version = Version.Parse(upgradeVersion);
                Version newVersion = new Version(
                    version.Major, version.Minor, version.Build, version.Revision + 1);

                appManifestFileContent = appManifestFileContent.Replace(
                    $"Version=\"{upgradeVersion}\"", $"Version=\"{newVersion}\"");
                File.WriteAllText(appManifestFilePath, appManifestFileContent);

                string[] serviceFolders = Directory.GetDirectories(appFolder);
                foreach (string serviceFolder in serviceFolders)
                {
                    string serviceManifestFilePath = Path.Combine(
                        serviceFolder, serviceManifestFileName);
                    string serviceManifestFileContent = File.ReadAllText(serviceManifestFilePath);
                    serviceManifestFileContent = serviceManifestFileContent.Replace(
                        $"Version=\"{upgradeVersion}\"", $"Version=\"{newVersion}\"");
                    File.WriteAllText(serviceManifestFilePath, serviceManifestFileContent);
                }
            }
        }
    }
}
