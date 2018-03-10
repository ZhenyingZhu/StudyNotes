using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class MyDerive1 : MyBase
    {
        public string Name { get; set; }

        public override void PrintId()
        {
            Console.WriteLine(@"MyDerive1 print Id: {0}", Id);
        }
    }
}
