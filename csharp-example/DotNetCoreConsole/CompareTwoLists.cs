using System;
using System.Collections.Generic;

namespace CompareTwoLists
{
    public class MyClass
    {
        public ICollection<Guid> IdList { get; set; }
    }

    public class TestMain
    {
        public static void testMain()
        {
            MyClass mc = new MyClass
            {
                IdList = new List<Guid> { new Guid() }
            };

            System.Console.WriteLine(String.Join(", ", mc.IdList));
        }
    }
}
