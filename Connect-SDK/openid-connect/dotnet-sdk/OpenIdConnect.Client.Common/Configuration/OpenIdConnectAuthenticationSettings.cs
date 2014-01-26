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
        /// The base URI to SDB Connect Identity Gateway (IdG)
        /// Example: https://id.services.telecom.pt
        /// </summary>
        public Uri SdbIdentityGatewayBaseUri { get; set; }

        /// <summary>
        /// The base URI to SDB Runtime.
        /// Example: https://services.telecom.pt
        /// </summary>
        public Uri SdbRuntimeBaseUri { get; set; }
    }
}