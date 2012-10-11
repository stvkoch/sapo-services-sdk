using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace PhotosSample
{
    public class SapoServicesHttpClient: HttpClient
    {
        private const string HostWithScheme = "http://services.sapo.pt/";

        private Uri _baseUri;
        public SapoServicesHttpClient(string serviceName, 
            string esbUsername, string esbPassword, string esbAccessKey): base()
        {
            this._baseUri = new Uri(HostWithScheme + serviceName);
            this.DefaultRequestHeaders.Authorization = 
                new AuthenticationHeaderValue("ESB", "ESBAccessKey="+esbAccessKey);
        }
    }
}
