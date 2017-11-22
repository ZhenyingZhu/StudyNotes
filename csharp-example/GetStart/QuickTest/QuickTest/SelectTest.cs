using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class SelectTest
    {
        public static void TestMain()
        {
            List<SelectTestClass> oldList = new List<SelectTestClass>()
            {
                new SelectTestClass(1, false),
                new SelectTestClass(2, true),
                new SelectTestClass(3, false),
            };

            IList<SelectTestClass> newList = oldList.Select(SelectTestClass.SelectSomeItems).ToList();

            foreach (var stc in newList)
            {
                Console.WriteLine(stc);
            }
        }
    }
}
