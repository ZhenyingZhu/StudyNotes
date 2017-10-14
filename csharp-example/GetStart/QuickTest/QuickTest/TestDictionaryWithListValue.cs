using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TestDictionaryWithListValue
    {
        public static void TestMain()
        {
            AddAnEntry();
        }

        private static void AddAnEntryWrong()
        {
            // This will throw NullEx
            Dictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
            dict["a"].Add(1);

            Console.WriteLine(dict["a"].Count);
            Console.WriteLine(dict["a"][0]);
        }

        private static void AddAnEntry()
        {
            Dictionary<string, List<int>> dict = new Dictionary<string, List<int>>();
            dict["a"].Add(1);

            Console.WriteLine(dict["a"].Count);
            Console.WriteLine(dict["a"][0]);
        }
    }
}
