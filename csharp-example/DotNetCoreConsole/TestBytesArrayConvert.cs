using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestBytesArrayConvert
{
    public class TestMain
    {
        public static void testMain()
        {
            string str = "apple";
            byte[] byteArray = Encoding.UTF8.GetBytes(str);

            string strVal = Encoding.UTF8.GetString(byteArray);
            System.Console.WriteLine(strVal);

            int intVal = BitConverter.ToInt32(byteArray, 0);
            System.Console.WriteLine(intVal);
        }
    }
}
