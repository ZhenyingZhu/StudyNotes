using System;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace DotNetCoreConsole
{
    public class SimpleTask : Task
    {
        // Seems like MSBuild work differently then the dotnet core projects.
        public override bool Execute()
        {
            Console.WriteLine("Hello World");
            return true;
        }
    }
}