using System.Collections.Generic;
using Newtonsoft.Json;

namespace OpenIdConnect.Client.Common.Representations
{
    /// <summary>
    /// The access_token request, as described in http://tools.ietf.org/html/rfc6749#page-29 and http://tools.ietf.org/html/rfc6749#section-3.2.1
    /// </summary>
    public class AccessTokenRequest
    {
        /// <summary>
        /// The grant_type. Should be set to "authorization_code" when using the code flow.
        /// </summary>
        [JsonProperty("grant_type")]
        public string GrantType { get; set; }

        /// <summary>
        /// The authorization code to exchange.
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// The original redirect_uri, for cross-check.
        /// </summary>
        [JsonProperty("redirect_uri")]
        public string RedirectUri { get; set; }

        /// <summary>
        /// The client_id of the caller application.
        /// </summary>
        [JsonProperty("client_id")]
        public string ClientId { get; set; }

        /// <summary>
        /// The client_secret of the caller application.
        /// </summary>
        [JsonProperty("client_secret")]
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets the request properties as KeyValuePairs, for easier integration with FormUrlEncoded requests.
        /// </summary>
        /// <returns>A set of KeyValuePairs for all the properties in the request</returns>
        public IEnumerable<KeyValuePair<string, string>> GetKeyValuePairs()
        {
            yield return new KeyValuePair<string, string>("grant_type", GrantType);
            yield return new KeyValuePair<string, string>("code", Code);
            yield return new KeyValuePair<string, string>("redirect_uri", RedirectUri);
            yield return new KeyValuePair<string, string>("client_id", ClientId);
            yield return new KeyValuePair<string, string>("client_secret", ClientSecret);
        } 
    }
}