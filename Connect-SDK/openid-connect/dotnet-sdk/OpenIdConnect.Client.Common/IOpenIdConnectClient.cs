using System.Collections.Generic;
using System.Threading.Tasks;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Common
{
    /// <summary>
    /// A client that will handle the require steps to perform the OpenId Connect flows.
    /// </summary>
    public interface IOpenIdConnectClient
    {
        /// <summary>
        /// Starts the SignIn flow.
        /// This will redirect the user to the SDB Connect IdG Endpoint, where the user will perform
        /// a series for redirect hops until authenticated. 
        /// Then, the user will be redirect to redirectUri to complete the SignIn at application level. 
        /// 
        /// The steps are:
        /// 1. Redirect to SDB Connect IdG to perform the sign in
        /// 2. The user performs a series of hops (transparent to the application) to complete the authentication at federation level.
        /// 3. Callback to the application sign in redirect URI, where the application receives a "code" (authorization code)
        /// 4. The application should then call the CompleteSignIn method to retrieve the access_token from the given "code"
        /// 
        /// The next step should be to exchange the received "code" for an access_token, using the CompleteSignIn method.
        /// </summary>
        /// <param name="redirectUri">
        /// The callback URI where the application will call the CompleteSignIn, to exchange the "code" for an "access_token".
        /// </param>
        /// <returns>
        /// An HTTP Redirect, to redirect the user to SDB Connect IdG and start the sign in process.
        /// </returns>
        HttpRedirectResponse StartSignIn(string redirectUri);

        /// <summary>
        /// Starts the SignOut flow. 
        /// You should start this step before cleaning up the user session, because this can be 
        /// done in the final redirect. 
        /// 
        /// The steps are:
        /// 1. Redirect to SDB Connect IdG to perform the sign out at federation level
        /// 2. Callback to the application sign out redirect URI to cleanup at application level
        /// </summary>
        /// <param name="accessTokenResponse">
        /// The token endpoint's access token response (when the user completed the sign in flow). 
        /// We need the id_token that was returned in this response.
        /// </param>
        /// <param name="redirectUri">
        /// The callback URI where the application will cleanup the user's session
        /// </param>
        /// <returns>
        /// An HTTP Redirect, to redirect the user to SDB Connect IdG or, in special cases, to the redirectUri itself.
        /// </returns>
        HttpRedirectResponse StartSignOut(AccessTokenResponse accessTokenResponse, string redirectUri);

        /// <summary>
        /// Completes the SignIn flow.
        /// This is the final step in the SignIn flow, that will exchange the authorization code for a access_token.
        /// At this point, the client will perform a "backend" request where it will authenticate itself on the token_endpoint, so the access_token can be generated.
        /// </summary>
        /// <param name="code">The "code" that was provided in the StartSignIn callback.</param>
        /// <param name="state">The "state" that was provided in the StartSignIn callback.</param>
        /// <param name="redirectUri">The original redirect URI that was provided to the StartSignIn method.</param>
        /// <returns>A future AccessContext object containing, among other things, the access_token and the id_token.</returns>
        Task<AccessTokenResponse> CompleteSignIn(string code, string state, string redirectUri);

        /// <summary>
        /// Obtains the user's claims, from the userInfo endpoint, given the user's access_token
        /// </summary>
        /// <param name="accessTokenResponse">
        /// The token endpoint's access token response (when the user completed the SignIn flow). 
        /// We need the access_token that was returned in this response.
        /// </param>
        /// <returns>
        /// A future dictionary, mapping claim types to their values. 
        /// The amount of claims, claim names and their value formats are all defined/configurable on SDB Connect IdG.
        /// 
        /// Typically the claim names are returned according to the OpenIdConnect standard. 
        /// For more information, see: http://openid.net/specs/openid-connect-basic-1_0-28.html#StandardClaims
        /// </returns>
        Task<IDictionary<string, string>> GetUserInfo(AccessTokenResponse accessTokenResponse);
    }
}
