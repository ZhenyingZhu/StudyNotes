using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DotNetCoreConsole
{
    public class TestLoadXmlElement
    {
        public static void testMain()
        {
            XElement localDiskXml = XElement.Load(@"example.xml");
            XElement firstNode = XElement.Parse(localDiskXml.FirstNode.ToString());

            Console.WriteLine(firstNode);
        }
    }
}
