using System;
using TestNugetLib;

namespace TestNugetConsumer
{
    class Program
    {
        static void Main(string[] args)
        {
            // The Nuget.config specifies a local source foo.bar
            // dotnet add package TestNugetLibZhenying
            TestNugetLib.Helper.PrintUtcTime();
        }
    }
}
