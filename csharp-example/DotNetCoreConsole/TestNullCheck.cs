using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestNullCheck
{
    public class TestMain
    {
        public static void testMain()
        {
            CouldBeNullClass a = new CouldBeNullClass() { name = "A",  valid = true };
            // So a?.IsValid could return null
            if (a.IsValid())
            {
                a?.PrintMessage();
            }
        }

        private class CouldBeNullClass
        {
            public string name { get; set; }

            public bool valid { get; set; }

            public bool IsValid()
            {
                return this.valid;
            }

            public void PrintMessage()
            {
                System.Console.WriteLine(string.Format("name: {0}", this.name));
            }
        }
    }
}
