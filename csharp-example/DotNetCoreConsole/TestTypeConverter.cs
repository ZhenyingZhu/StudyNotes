using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConsole
{
    public class TestTypeConverterBehavior
    {
        public static void testMain()
        {
            Type myClassType = typeof(MyClass);
            TypeConverter converter = TypeDescriptor.GetConverter(myClassType);
            Console.WriteLine(converter.CanConvertFrom(typeof(string)));

            TypeConverter intConverter = TypeDescriptor.GetConverter(typeof(int));
            Console.WriteLine(intConverter.CanConvertFrom(typeof(string)));
        }

        private class MyClass
        {
            public int MyVal { get; set; }
        }
    }
}
