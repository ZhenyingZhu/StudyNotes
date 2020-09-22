using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestExceptLinkOnStruct
{
    public class TestMain
    {
        private static void testSimpleType()
        {
            List<string> list1 = new List<string>{"a", "b"};

            List<string> list2 = new List<string>{"a", "c"};

            List<string> list1MinusList2 = list1.Except(list2).ToList();

            System.Console.WriteLine(string.Join(",", list1MinusList2));
        }

        private static void testEquals()
        {
            MetaData m1 = new MetaData { name = "a" };
            MetaData m2 = new MetaData { name = "a" };
            System.Console.WriteLine(m1.Equals(m2));
        }

        private static void testComplexType()
        {
            List<MetaData> list1 = new List<MetaData>
            {
                new MetaData { name = "a" },
                new MetaData { name = "b", val = 2 },
                new MetaData { name = "d", val = 4 },
            };

            List<MetaData> list2 = new List<MetaData>
            {
                new MetaData { name = "a", val = 1 },
                new MetaData { name = "b" },
                new MetaData { name = "c" },
            };

            List<string> list1MinusList2 = list1.Except(list2).Select(m => m.name).ToList();

            System.Console.WriteLine(string.Join(",", list1MinusList2));

            IEnumerable<MetaData> list2MinusList1 = list2.Except(list1);
            System.Console.WriteLine(string.Join(",", list2MinusList1));
        }

        public static void testMain()
        {
            testComplexType();
        }

        private class MetaData
        {
            public string name { get; set; }
            public int? val { get; set; }

            public override bool Equals(Object obj)
            {
                //Check for null and compare run-time types.
                if ((obj == null) || !this.GetType().Equals(obj.GetType()))
                {
                    return false;
                }
                else
                {
                    MetaData m = (MetaData) obj;
                    return name.Equals(m.name);
                }
            }

            public override int GetHashCode()
            {
                return this.name.GetHashCode();
            }

            public override string ToString()
            {
                return string.Format("name: {0}, val: {1}", this.name, this.val);
            }
        }
    }
}
