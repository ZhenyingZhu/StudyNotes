using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Schema;

namespace DotNetCoreConsole
{
    public class TestReadWriteXMLSchema
    {
        public static void testMain()
        {
            try
            {
                var readSettings = new XmlReaderSettings()
                {
                    IgnoreComments = false
                };
                XmlReader reader = XmlReader.Create("example.xsd");
                XmlSchema myschema = XmlSchema.Read(reader, ValidationCallback);
                myschema.Write(Console.Out);

                FileStream file = new FileStream("writebackexample.xsd", FileMode.Create, FileAccess.ReadWrite);
                XmlWriterSettings writeSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    OmitXmlDeclaration = false,
                    NewLineOnAttributes = true
                };
                XmlWriter xwriter = XmlWriter.Create(file, writeSettings);
                myschema.Write(xwriter);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }

        private static void ValidationCallback(object sender, ValidationEventArgs args)
        {
            if (args.Severity == XmlSeverityType.Warning)
                Console.Write("WARNING: ");
            else if (args.Severity == XmlSeverityType.Error)
                Console.Write("ERROR: ");

            Console.WriteLine(args.Message);
        }
    }
}
