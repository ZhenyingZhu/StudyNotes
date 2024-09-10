using System;
using System.Collections.Generic;

namespace DotNetCoreConsole
{
    public class ICollectionClassInit
    {
        public static void TestMain()
        {
            MyClass mc = new MyClass
            {
                IdList = new List<Guid> { new Guid() }
            };

            System.Console.WriteLine(String.Join(", ", mc.IdList));
        }

        private class MyClass
        {
            public ICollection<Guid> IdList { get; set; }
        }
    }
}
