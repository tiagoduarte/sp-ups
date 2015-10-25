using microsoft.com.webservices.SharePointPortalServer.UserProfileService;
using System;
using System.ServiceModel;

namespace SPUPS
{
    public class UserProfileServiceHelper
    {
        public UserProfileServiceHelper()
        {

        }

        /// <summary>
        /// displays some properties of the user profile service, should they exist
        /// </summary>
        /// <param name="webUrl"></param>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        public void DisplayProperties(string webUrl, string domain, string username, string[] properties)
        {
            //, "UserProfile_GUID"};
            
            //setup binding
            BasicHttpBinding httpBinding = new BasicHttpBinding();
            httpBinding.Security.Mode = BasicHttpSecurityMode.TransportCredentialOnly;
            httpBinding.Security.Transport.ClientCredentialType = HttpClientCredentialType.Ntlm;
            //httpBinding.MessageEncoding = WSMessageEncoding.Text;

            //fix buffer size issues
            httpBinding.MaxReceivedMessageSize = 2147483647;
            httpBinding.MaxBufferSize = 2147483647;

            //setup endpoint. combine urls
            //http://weburl/_vti_bin/UserProfileService.asmx
            EndpointAddress address = new EndpointAddress(new Uri(new Uri(webUrl), "/_vti_bin/UserProfileService.asmx"));
                        
            //start webservice
            UserProfileServiceSoapClient client = new UserProfileServiceSoapClient(httpBinding, address);
            client.ClientCredentials.Windows.ClientCredential = System.Net.CredentialCache.DefaultNetworkCredentials;
            //client.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("username", "password", "domain");
            client.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;

            PropertyData[] data;
            try
            {
                data = client.GetUserProfileByName(domain + "\\" + username);
                if(data != null)
                {
                    foreach (string property in properties)
                    {
                        try
                        {
                            PropertyData prop = client.GetUserPropertyByAccountName(domain + "\\" + username, property);

                            if (prop != null)
                            {
                                foreach (ValueData value in prop.Values)
                                {
                                    Console.WriteLine(property + " is \t   " + value.Value.ToString());
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            catch(FaultException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }

    }
}
