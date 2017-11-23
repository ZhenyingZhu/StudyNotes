using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class SelectTest
    {
        private static void SelectByStaticMathod(IList<SelectTestClass> oldList)
        {
            IList<SelectTestClass> newList = oldList.Select(SelectTestClass.SelectSomeItems).ToList();

            foreach (var stc in newList)
            {
                Console.WriteLine(stc);
            }
        }

        private static void SelectByGroup(IList<SelectTestClass> oldList)
        {
            var listQuery = from stc in oldList
                group stc by stc.Id
                into stcGroup
                select stcGroup;

            foreach (IGrouping<int, SelectTestClass> selectTestClasses in listQuery)
            {
                Console.WriteLine(selectTestClasses.Key);
                // Explicit type for student could also be used here.
                foreach (var student in selectTestClasses)
                {
                    Console.WriteLine(student);
                }
            }
        }

        public static void TestMain()
        {
            List<SelectTestClass> oldList = new List<SelectTestClass>()
            {
                new SelectTestClass(1, false),
                new SelectTestClass(2, true),
                new SelectTestClass(3, false),
                new SelectTestClass(1, true),
            };

            SelectByGroup(oldList);


        }
    }
}
