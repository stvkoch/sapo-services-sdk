using Newtonsoft.Json;

namespace OpenIdConnect.Client.Common.Representations
{
    /// <summary>
    /// The token_endpoint response message.
    /// This contains the basic information that the application will need to authenticate on behalf of the user.
    /// 
    /// For more information, see: http://tools.ietf.org/html/rfc6749#section-5.1
    /// </summary>
    public class AccessTokenResponse
    {
        /// <summary>
        /// The access token issued by the authorization server.
        /// 
        /// This will be passed along in the Authorization header, 
        /// with the Bearer scheme, on requests made on behalf of the user.
        /// </summary>
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        /// <summary>
        /// The type of the token issued as described in http://tools.ietf.org/html/rfc6749#section-7.1.
        /// Value is case insensitive.
        /// 
        /// The type of the access_token, that should be used to determine the way in which the access_token 
        /// should be used. Typically this defaults to "Bearer".
        /// </summary>
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        /// <summary>
        /// The lifetime in seconds of the access token.  For 
        /// example, the value "3600" denotes that the access token will
        /// expire in one hour from the time the response was generated.
        /// </summary>
        [JsonProperty("expires_in")]
        public string ExpiresIn { get; set; }

        /// <summary>
        /// The refresh_token.
        /// 
        /// OPTIONAL.  The refresh token, which can be used to obtain new 
        /// access tokens using the same authorization grant as described 
        /// in http://tools.ietf.org/html/rfc6749#section-6.
        /// </summary>
        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        /// <summary>
        /// The authorization scope.
        /// 
        /// OPTIONAL, if identical to the scope requested by the client; 
        /// otherwise, REQUIRED.  The scope of the access token as 
        /// described by http://tools.ietf.org/html/rfc6749#section-3.3.
        /// </summary>
        [JsonProperty("scope")]
        public string Scope { get; set; }

        /// <summary>
        /// The ID Token is a security token that contains Claims about the authentication event and 
        /// other requested Claims. The ID Token is represented as a JSON Web Token (JWT).
        /// 
        /// http://openid.net/specs/openid-connect-basic-1_0-28.html#id_token
        /// http://openid.net/specs/openid-connect-basic-1_0-28.html#JWT
        /// </summary>
        [JsonProperty("id_token")]
        public string IdToken { get; set; }
    }
}