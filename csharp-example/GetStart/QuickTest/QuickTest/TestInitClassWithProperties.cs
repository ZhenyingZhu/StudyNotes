using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class TestInitClassWithProperties
    {
        public void TestMain()
        {
            MyDerive1 myDerive1 = new MyDerive1()
            {
                Id = 1,
                Name = "MyDerive1"
            };

            Console.WriteLine("See if it works: " + myDerive1.Name + myDerive1.Id);
            
            Console.ReadLine();
        }
    }
}
