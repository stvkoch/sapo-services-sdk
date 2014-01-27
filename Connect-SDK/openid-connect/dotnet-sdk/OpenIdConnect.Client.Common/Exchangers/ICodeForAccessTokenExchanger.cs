using System;
using System.Threading.Tasks;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Common.Exchangers
{
    /// <summary>
    /// Represents an entity capable of calling the token_endpoint to exchange a code for an access_token
    /// </summary>
    public interface ICodeForAccessTokenExchanger
    {
        /// <summary>
        /// Exchanges a code for an access_token.
        /// </summary>
        /// <param name="baseUri">SDB Runtime base URI</param>
        /// <param name="request">The access_token request, containing the authorization code and the client authentication info</param>
        /// <returns>A future access_token response, containing the access_token, id_token, and others.</returns>
        Task<AccessTokenResponse> Exchange(Uri baseUri, AccessTokenRequest request);
    }
}