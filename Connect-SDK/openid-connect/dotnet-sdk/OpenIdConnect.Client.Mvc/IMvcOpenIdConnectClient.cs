using System.Web.Mvc;
using OpenIdConnect.Client.Common;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Mvc
{
    /// <summary>
    /// An OpenIdConnect client with types and features that are MVC-specific
    /// </summary>
    public interface IMvcOpenIdConnectClient : IOpenIdConnectClient
    {
        /// <summary>
        /// The same as StartSignIn, but returns an ActionResult.
        /// </summary>
        /// <param name="redirectUri">
        /// The callback URI where the application will call the CompleteSignIn, to exchange the "code" for an "access_token".
        /// </param>
        /// <returns>
        /// A Redirect ActionResult, to redirect the user to SDB Connect IdG and start the sign in process.
        /// </returns>
        ActionResult StartMvcSignIn(string redirectUri);

        /// <summary>
        /// The same as StartSignOut, but returns an ActionResult
        /// </summary>
        /// <param name="accessTokenResponse">
        /// The token endpoint's access token response (when the user completed the sign in flow). 
        /// We need the id_token that was returned in this response.
        /// </param>
        /// <param name="redirectUri">
        /// The callback URI where the application will cleanup the user's session
        /// </param>
        /// <returns>
        /// A Redirect ActionResult, to redirect the user to SDB Connect IdG or, in special cases, to the redirectUri itself.
        /// </returns>
        ActionResult StartMvcSignOut(AccessTokenResponse accessTokenResponse, string redirectUri);
    }
}
