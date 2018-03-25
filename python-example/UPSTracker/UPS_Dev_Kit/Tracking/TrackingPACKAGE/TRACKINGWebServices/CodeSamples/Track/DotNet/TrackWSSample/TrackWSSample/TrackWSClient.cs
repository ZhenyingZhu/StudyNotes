using System;
using System.Collections.Generic;
using System.Text;
using TrackWSSample.TrackWebReference;
using System.ServiceModel;

namespace TrackWSSample
{
    class TrackWSClient
    {
      static void Main()
        {
            try
            {
                TrackService track = new TrackService();
                TrackRequest tr = new TrackRequest();
                UPSSecurity upss = new UPSSecurity();
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = "Your access license number";
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = "Your username";
                upssUsrNameToken.Password = "Your password";
                upss.UsernameToken = upssUsrNameToken;
                track.UPSSecurityValue = upss;
                RequestType request = new RequestType();
                String[] requestOption = { "15" };
                request.RequestOption = requestOption;
                tr.Request = request;
                tr.InquiryNumber = "Your track inquiry number";
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                TrackResponse trackResponse = track.ProcessTrack(tr);
                Console.WriteLine("The transaction was a " + trackResponse.Response.ResponseStatus.Description);
                Console.WriteLine("Shipment Service " + trackResponse.Shipment[0].Service.Description);
                Console.ReadKey();
            }
            catch (System.Web.Services.Protocols.SoapException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("---------Track Web Service returns error----------------");
                Console.WriteLine("---------\"Hard\" is user error \"Transient\" is system error----------------");
                Console.WriteLine("SoapException Message= " + ex.Message);
                Console.WriteLine("");
                Console.WriteLine("SoapException Category:Code:Message= " + ex.Detail.LastChild.InnerText);
                Console.WriteLine("");
                Console.WriteLine("SoapException XML String for all= " + ex.Detail.LastChild.OuterXml);
                Console.WriteLine("");
                Console.WriteLine("SoapException StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");
                Console.WriteLine("");
            }
            catch (System.ServiceModel.CommunicationException ex)
            {
                Console.WriteLine("");
                Console.WriteLine("--------------------");
                Console.WriteLine("CommunicationException= " + ex.Message);
                Console.WriteLine("CommunicationException-StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");
                Console.WriteLine("");

            }
            catch (Exception ex)
            {
                Console.WriteLine("");
                Console.WriteLine("-------------------------");
                Console.WriteLine(" General Exception= " + ex.Message);
                Console.WriteLine(" General Exception-StackTrace= " + ex.StackTrace);
                Console.WriteLine("-------------------------");

            }
            finally
            {
                Console.ReadKey();
            }
           
       }
    }
}
