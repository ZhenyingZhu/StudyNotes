using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace GetCert
{
    class Program
    {
        static void Main(string[] args)
        {
            // Console.WriteLine("\r\nExists Certs Name and Location");
            // Console.WriteLine("------ ----- -------------------------");

            // foreach (StoreLocation storeLocation in (StoreLocation[])Enum.GetValues(typeof(StoreLocation)))
            StoreLocation storeLocation = StoreLocation.LocalMachine;
            {
                // foreach (StoreName storeName in (StoreName[])Enum.GetValues(typeof(StoreName)))
                StoreName storeName = StoreName.My;
                {
                    X509Store store = new X509Store(storeName, storeLocation);

                    try
                    {
                        store.Open(OpenFlags.OpenExistingOnly);

                        // Console.WriteLine("Yes    {0,4}  {1}, {2}", store.Certificates.Count, store.Name, store.Location);

                        foreach (var cert in store.Certificates)
                        {
                            Console.WriteLine(cert.FriendlyName);
                        }
                    }
                    catch (CryptographicException)
                    {
                        // Console.WriteLine("No           {0}, {1}", store.Name, store.Location);
                        Console.WriteLine("Store not exist. Name: {0}, Location: {1}.", store.Name, store.Location);
                    }
                }
                Console.WriteLine();
            }
        }
    }
}
