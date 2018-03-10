using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class MyDerive2: MyBase
    {
        public override void PrintId()
        {
            Console.WriteLine(@"MyDerive2 print Id: {0}", Id);
        }
    }
}
