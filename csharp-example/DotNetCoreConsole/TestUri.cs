using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestUri
    {
        public static void testMain()
        {
            Uri apiUri = new Uri("https://myhost.com?api-version=1.0");
            string resourcePath = "MyResource";
            
            System.UriBuilder uriBuilder = new System.UriBuilder(apiUri);
            uriBuilder.Path += resourcePath;
            
            Console.WriteLine(uriBuilder.Uri.ToString());
        }
    }
}
