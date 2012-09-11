using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Videos.VideosServiceReference;

namespace Videos.SapoServices.Utils
{
    public sealed class EnsureCredentialsUseContext : IDisposable
    {
        private const string Esbcredentials = "ESBCredentials";
        private const string HttpServicesSapoPtDefinitions = "http://services.sapo.pt/definitions";
        private const string Esbaccesskey = "ESBAccessKey";
        private const string HttpServicesSapoPtMetadataMarket = "http://services.sapo.pt/Metadata/Market";

        private readonly OperationContextScope _scope;

        public EnsureCredentialsUseContext(
            string username, string password, string accessKey, VideosSoapSecureClient client)
        {
            EsbCredentials credentials = new EsbCredentials
            {
                Username = username,
                Password = password,
            };

            _scope = new OperationContextScope(client.InnerChannel);
            MessageHeader<EsbCredentials> credentialsMessageHeader =
                new MessageHeader<EsbCredentials>(credentials);
            MessageHeader<string> accessKeyMessageHeader =
                new MessageHeader<string>(accessKey);

            OperationContext.Current.OutgoingMessageHeaders
                .Add(credentialsMessageHeader
                .GetUntypedHeader(Esbcredentials, HttpServicesSapoPtDefinitions));

            OperationContext.Current.OutgoingMessageHeaders
                .Add(accessKeyMessageHeader
                .GetUntypedHeader(Esbaccesskey, HttpServicesSapoPtMetadataMarket));
        }

        public void Dispose()
        {
            this._scope.Dispose();
        }
    }

}
