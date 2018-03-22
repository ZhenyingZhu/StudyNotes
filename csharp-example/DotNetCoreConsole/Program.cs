using System;
using System.Collections.Generic;

namespace DotNetCoreConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> testList = new List<string>(){"apple", "banana"};
            string result = string.Format("Print list: {0}", string.Join(",", testList));

            Console.WriteLine(result);
        }
    }
}
