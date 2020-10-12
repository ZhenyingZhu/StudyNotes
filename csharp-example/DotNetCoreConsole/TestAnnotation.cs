using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DotNetCoreConsole
{
    public class TestAnnotation
    {
        public static void testMain()
        {
            Type myClassType = typeof(MyClass);
            IEnumerable<PropertyInfo> props = myClassType.GetProperties();
            MyAttrAttribute myAttr = Attribute.GetCustomAttribute(props.First(), typeof(MyAttrAttribute)) as MyAttrAttribute;
            Console.WriteLine(myAttr.Value);
        }

        private sealed class MyAttrAttribute : Attribute
        {
            public MyAttrAttribute(int val)
            {
                this.Value = val;
            }

            public int Value { get; set; }
        }

        private class MyClass
        {
            [MyAttr(1)]
            public int MyVal { get; set; }
        }
    }
}
