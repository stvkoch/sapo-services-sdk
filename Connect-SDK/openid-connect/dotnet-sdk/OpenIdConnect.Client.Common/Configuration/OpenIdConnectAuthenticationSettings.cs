using System;

namespace OpenIdConnect.Client.Common
{
    /// <summary>
    /// The OpenId Connect client and server settings
    /// </summary>
    public class OpenIdConnectAuthenticationSettings
    {
        /// <summary>
        /// The client_id. 
        /// This is provided to you after registering a relying party with SDB Connect IdG.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// The client_secret. 
        /// This is provided to you after registering a relying party with SDB Connect IdG.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// The access scope. 
        /// This is provided to you after registering a relying party with SDB Connect IdG.
        /// </summary>
        public string Scope { get; set; }

        /// <summary>
        /// The base URI to Authorization Endpoint
        /// Example: https://id.services.telecom.pt/oic/
        /// </summary>
        public Uri AuthorizationEndpoint { get; set; }

        /// <summary>
        /// The base URI to the token endpoint.
        /// Example: https://services.telecom.pt/connect/token_endpoint/access_token
        /// </summary>
        public Uri TokenEndpoint { get; set; }
        
        /// <summary>
        /// The base URI to the UserInfo endpoint.
        /// Example: https://services.telecom.pt/connect/oic/userinfo/
        /// </summary>
        public Uri UserInfoEndpoint { get; set; }
    }
}