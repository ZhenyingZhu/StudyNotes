using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TryNewList
    {
        public static void TestMain()
        {
            IList<int> lst = new List<int>(
                // null); cannot be null
                );

            foreach (int v in lst)
            {
                Console.WriteLine(v);
            }
        }
    }
}
