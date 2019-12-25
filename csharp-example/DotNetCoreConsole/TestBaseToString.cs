using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace TestBaseToString
{
    public class MyBase
    {
        public int BaseId { get; set; }

        public MyBase(int baseId)
        {
            this.BaseId = baseId;
        }

        public override string ToString()
        {
            // https://stackoverflow.com/questions/852181/c-printing-all-properties-of-an-object
            string str = string.Empty;
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(this))
            {
                string name = descriptor.Name;
                object value = descriptor.GetValue(this);
                str += string.Format("{0}={1};", name, value);
            }

            // https://stackoverflow.com/questions/17091115/type-getproperties-returning-nothing/17091325
            // Notice that if the property is not defined as public, it won't print.

            return str;
        }
    }

    public class MyChild: MyBase
    {
        public string MyValue { get; set; }

        public MyChild(string value, int id): base(id)
        {
            this.MyValue = value;
        }
    }

    public class TestMain
    {
        public static void testMain()
        {
            TestBaseToString.MyChild c = new TestBaseToString.MyChild("child", 1);
            System.Console.WriteLine("child value:" + c);
        }
    }
}
