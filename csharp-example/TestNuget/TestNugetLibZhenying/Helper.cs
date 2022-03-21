using System;

namespace TestNugetLib
{
    public class Helper
    {
        public static void PrintUtcTime()
        {
            // Created a source foo.bar:
            // nuget sources add -name foo.bar -source C:\NuGet\local -username foo -password bar -StorePasswordInClearText -configfile Nuget.config
            // Publish the package:
            // dotnet pack
            // nuget add .\bin\Debug\TestNugetLibZhenying.1.0.0.nupkg -source C:\NuGet\local
            Console.WriteLine(DateTime.UtcNow);
        }
    }
}
