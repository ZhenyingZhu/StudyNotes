using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace TestJsonParseDict
{
    public class TestMain
    {
        private static string Serialize(Dictionary<Tuple<string, string>, string> dict)
        {
            return JsonConvert.SerializeObject(dict);
        }

        private static string Serialize2(Tuple<Guid, string> val)
        {
            return JsonConvert.SerializeObject(val);
        }

        private static Dictionary<Tuple<Guid, string>, string> Parse(string jsonStr)
        {
            var result = JsonConvert.DeserializeObject<Dictionary<Tuple<Guid, string>, string>>(jsonStr);
            return result;
        }

        private static Tuple<string, string> Parse2(string jsonStr)
        {
            var result =  JsonConvert.DeserializeObject<Tuple<string, string>>(jsonStr);
            return result;
        }

        private static string Parse3(string jsonStr)
        {
            var result =  JsonConvert.DeserializeObject<string>(jsonStr);
            return result;
        }

        public static void testMain()
        {
            Guid gid = Guid.Parse("61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3");
            var dict = new Dictionary<Tuple<Guid, string>, string>
            {
                { Tuple.Create(gid, "key1"), "val1" },
                { Tuple.Create(gid, "key2"), "val2" }
            };

            //string jsonStr = @"{'Item1':'61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3','Item2':'key1'}";
            //System.Console.WriteLine(jsonStr);
            //var deTup = Parse2(jsonStr);
            //System.Console.WriteLine(deTup);

            //string jsonStr = Serialize(dict);
            //string jsonStr = @"{""{'Item1':'61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3','Item2':'key1'}"":'val1'}";
            //string jsonStr = @"{""{'Item1':'61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3','Item2':'key1'}"":""val1""}";
            //string jsonStr = @"{""(61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3,key1)"":""val1""}";
            string jsonStr = @"{""{Item1:'61bdb951-bda3-42d0-a7bd-7b2f91ef1bd3',Item2:'key1'}"":'val1'}";
            System.Console.WriteLine(jsonStr);
            var deDict = Parse(jsonStr);
            System.Console.WriteLine(deDict);
        }
    }
}
