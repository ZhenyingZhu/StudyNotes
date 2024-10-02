using System;
using System.Collections.Generic;
using System.Linq;

namespace DotNetCoreConsole
{
    public class TestUri
    {
        public static void TestMain()
        {
            Uri apiUri = new Uri("https://myhost.com?api-version=1.0");
            string resourcePath = "MyResource";
            
            System.UriBuilder uriBuilder = new System.UriBuilder(apiUri);
            uriBuilder.Path += resourcePath;
            
            Console.WriteLine(uriBuilder.Uri.ToString());
        }

        public static string CraftUri()
        {
            Uri uri1 = new Uri("http://apple/banana");
            Uri uri2 = new Uri(uri1, "api");

            // banana is removed.
            return uri2.AbsoluteUri;
        }
    }
}
