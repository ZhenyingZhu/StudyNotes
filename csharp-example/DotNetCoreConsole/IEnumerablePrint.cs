using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class IEnumerablePrint
    {
        public static void testMain()
        {
            MyClass mc1 = new MyClass{id = 1};
            MyClass mc2 = new MyClass{id = 2};

            IEnumerable<MyClass> mcList = new List<MyClass> {mc1, mc2};

            System.Console.WriteLine("[" + String.Join(", ", mcList) + "]");

            List<int> idList = mcList.Select(x => x.id).ToList();
            System.Console.WriteLine("[" + String.Join(", ", idList) + "]");
        }
        
        private class MyClass
        {
            private const string StringTemplate = "Id={0}";
            public int id { get; set; }

            public override string ToString()
            {
                return string.Format(StringTemplate, this.id);
            }
        }
    }
}
