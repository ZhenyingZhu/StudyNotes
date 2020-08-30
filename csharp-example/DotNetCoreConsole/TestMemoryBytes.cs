using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace TestMemoryBytes
{
    public class TestMain
    {
        public static void testMain()
        {
            var stream = new MemoryStream();
            using (BinaryWriter writer = new BinaryWriter(stream))
            {
                writer.Write("first");
                writer.Write(1);
                writer.Flush();
            }
            byte[] valuesArray = stream.ToArray();

            using (BinaryReader reader = new BinaryReader(new MemoryStream(valuesArray)))
            {
                Console.WriteLine(reader.ReadString());
                Console.WriteLine(reader.ReadInt32());
            }
        }
    }
}
