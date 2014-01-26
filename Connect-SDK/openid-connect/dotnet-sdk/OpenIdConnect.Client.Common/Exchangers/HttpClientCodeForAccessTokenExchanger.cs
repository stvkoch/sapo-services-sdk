using System;
using System.Net.Http;
using System.Threading.Tasks;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Common.Exchangers
{
    /// <summary>
    /// An exchanger based on HttpClient
    /// </summary>
    public class HttpClientCodeForAccessTokenExchanger : ICodeForAccessTokenExchanger
    {
        private const string TokenEndpointRelativeUri = "connect/oauth2/token_endpoint/";
        private const string AccessTokenResourceName = "access_token";

        /// <summary>
        /// Exchanges a code for an access_token.
        /// </summary>
        /// <param name="baseUri">SDB Runtime base URI</param>
        /// <param name="request">The access_token request, containing the authorization code and the client authentication info</param>
        /// <returns>A future access_token response, containing the access_token, id_token, and others.</returns>
        public async Task<AccessTokenResponse> Exchange(Uri baseUri, AccessTokenRequest request)
        {
            using (var client = new HttpClient { BaseAddress = new Uri(baseUri, TokenEndpointRelativeUri) })
            {
                var response = await client.PostAsync(AccessTokenResourceName, 
                    new FormUrlEncodedContent(request.GetKeyValuePairs()))
                    .ConfigureAwait(false);

                // TODO process errors according to the OAuth2 standard

                var accessTokenResponse = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadAsAsync<AccessTokenResponse>()
                    .ConfigureAwait(false);

                return accessTokenResponse;
            }
        }
    }
}