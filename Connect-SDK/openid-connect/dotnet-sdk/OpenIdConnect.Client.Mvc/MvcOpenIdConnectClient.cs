using System.Web.Mvc;
using System.Web.SessionState;
using OpenIdConnect.Client.Common;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Mvc
{
    /// <summary>
    /// An MVC-specific OpenIdConnect Client. This reuses most of OpenIdConnectClient implementation.
    /// </summary>
    public class MvcOpenIdConnectClient : OpenIdConnectClient, IMvcOpenIdConnectClient
    {
        /// <summary>
        /// Creates a new MvcOpenIdConnectClient, given its configuration
        /// </summary>
        /// <param name="openIdSettings">
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </param>
        public MvcOpenIdConnectClient(OpenIdConnectAuthenticationSettings openIdSettings)
            : base(openIdSettings, new SystemWebSessionStateStore())
        {
        }

        /// <summary>
        /// Creates a new MvcOpenIdConnectClient, given its configuration and a custom Session State Store
        /// </summary>
        /// <param name="openIdSettings">
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </param>
        /// <param name="sessionStateStore">The session state store to use internally</param>
        public MvcOpenIdConnectClient(
            OpenIdConnectAuthenticationSettings openIdSettings, 
            ISessionStateStore sessionStateStore)
            : base(openIdSettings, sessionStateStore)
        {
        }

        /// <summary>
        /// Creates a new MvcOpenIdConnectClient, given its configuration with the given HttpSessionState store.
        /// </summary>
        /// <param name="openIdSettings">
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </param>
        /// <param name="sessionStateStore">The HttpSessionState to use internally in SystemWebSessionStateStore</param>
        public MvcOpenIdConnectClient(
            OpenIdConnectAuthenticationSettings openIdSettings,
            HttpSessionState sessionStateStore)
            : base(openIdSettings, new SystemWebSessionStateStore(sessionStateStore))
        {
        }

        /// <summary>
        /// The same as StartSignIn, but returns an ActionResult.
        /// </summary>
        /// <param name="redirectUri">
        /// The callback URI where the application will call the CompleteSignIn, to exchange the "code" for an "access_token".
        /// </param>
        /// <returns>
        /// A Redirect ActionResult, to redirect the user to SDB Connect IdG and start the sign in process.
        /// </returns>
        public ActionResult StartMvcSignIn(string redirectUri)
        {
            var redirectResponse = base.StartSignIn(redirectUri);
            return new RedirectResult(redirectResponse.Location.ToString());
        }

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
        public ActionResult StartMvcSignOut(AccessTokenResponse accessTokenResponse, string redirectUri)
        {
            var redirectResponse = base.StartSignOut(accessTokenResponse, redirectUri);
            return new RedirectResult(redirectResponse.Location.ToString());
        }
    }
}