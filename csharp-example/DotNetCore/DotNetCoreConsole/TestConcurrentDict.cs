using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;

namespace DotNetCoreConsole
{
    public class TestConcurrentDict
    {
        private readonly ConcurrentDictionary<string, int> conDict
            = new ConcurrentDictionary<string, int>();
        public static void TestMain()
        {
            TestConcurrentDict c = new TestConcurrentDict();
            c.Test();
        }

        private void Test()
        {
            Console.WriteLine(this.conDict.TryAdd("apple", 0));
            Console.WriteLine(this.conDict.TryAdd("banana", 2));
            Console.WriteLine(this.conDict.AddOrUpdate("apple", 1, (s, i) => 1));
            Console.WriteLine(this.conDict["apple"]);
            Console.WriteLine(this.conDict["banana"]);
        }
    }
}
