using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Management.Automation;

namespace DotNetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
                using (PowerShell powerShell = PowerShell.Create())
                {
                    // Add a cmdlet to the PowerShell pipeline
                    powerShell.AddCommand("Get-Process");

                    // Optionally, add parameters if needed
                    // powerShell.AddParameter("Name", "notepad");

                    // Execute the command
                    var results = powerShell.Invoke();

                    // Process and display the results
                    foreach (var result in results)
                    {
                        Console.WriteLine(result);
                    }
                }
        }
    }
}
