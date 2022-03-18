using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class InitListWithValues
    {
        public static async void TestMain()
        {
            List<int> a = new List<int>{1, 2, 3};
            List<int> b = new List<int>{4, 5, 6};
            List<int> c = new List<int>{7, 8};

            List<int> all = a.Concat(b).Concat(c).ToList();

            List<int> several = all.Skip(4).ToList();

            Console.WriteLine(string.Join(",", several));
        }
    }
}
