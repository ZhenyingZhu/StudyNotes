using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class IfBlockOutputValue
    {
        private Dictionary<string, string> dict = new Dictionary<string, string>();

        public static void TestMain()
        {
            IfBlockOutputValue i = new IfBlockOutputValue();
            i.dict.Add("a", "b");

            string v;
            if (i.GetValue("a", out v))
            {
                Console.WriteLine(v);
            }
        }

        private bool GetValue(string key, out string value)
        {
            return this.dict.TryGetValue(key, out value);
        }
    }

}
