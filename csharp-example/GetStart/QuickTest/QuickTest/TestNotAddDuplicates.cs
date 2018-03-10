using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TestNotAddDuplicates
    {
        public static void TestMain()
        {
            List<MyBase> myList = new List<MyBase>();

            HashSet<Type> existingTypes = new HashSet<Type>();

            Action<MyBase> addNoDupType = (b) =>
            {
                if (!existingTypes.Contains(b.GetType()))
                {
                    myList.Add(b);
                    existingTypes.Add(b.GetType());
                }
            };

            MyBase derive1Inst1 = new MyDerive1();
            MyBase derive1Inst2 = new MyDerive1();
            MyBase derive2Inst = new MyDerive2();

            addNoDupType(derive1Inst1);
            addNoDupType(derive1Inst2);
            addNoDupType(derive2Inst);

            foreach (var myBase in myList)
            {
                myBase.PrintId();
            }
        }
    }
}
