using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using OpenIdConnect.Client.Common.Exceptions;
using OpenIdConnect.Client.Common.Exchangers;
using OpenIdConnect.Client.Common.Representations;

namespace OpenIdConnect.Client.Common
{
    /// <summary>
    /// This client will handle the require steps to perform the user's SignIn and SignOut flows.
    /// 
    /// Note that most flows require a Start (redirect to somewhere else) and a Complete invocation, 
    /// that should be done in the endpoint that is provided in the "redirectUri" parameter.
    /// </summary>
    public class OpenIdConnectClient : IOpenIdConnectClient
    {
        /// <summary>
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </summary>
        private readonly OpenIdConnectAuthenticationSettings _openIdSettings;

        /// <summary>
        /// A session store that will be used to save the state parameters, for CSRF protection.
        /// </summary>
        private readonly ISessionStateStore _sessionSessionStateStore;

        private readonly ICodeForAccessTokenExchanger _codeForAccessTokenExchanger;

        /// <summary>
        /// The state key in the IStateStore
        /// </summary>
        public const string UserLoginStateKey = "UserLoginState";

        /// <summary>
        /// Creates a new OpenIdConnectAuthenticationHandler.
        /// </summary>
        /// <param name="openIdSettings">
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </param>
        /// <param name="sessionSessionStateStore">
        /// A session store that will be used to save the state parameters, for CSRF protection.
        /// </param>
        /// <param name="codeForAccessTokenExchanger">
        /// A instance of someone that can exchange authorization codes for access tokens.
        /// </param>
        public OpenIdConnectClient(
            OpenIdConnectAuthenticationSettings openIdSettings,
            ISessionStateStore sessionSessionStateStore,
            ICodeForAccessTokenExchanger codeForAccessTokenExchanger)
        {
            _openIdSettings = openIdSettings;
            _sessionSessionStateStore = sessionSessionStateStore;
            _codeForAccessTokenExchanger = codeForAccessTokenExchanger;
        }

        /// <summary>
        /// Creates a new OpenIdConnectAuthenticationHandler with the default exchanger (uses HttpClient).
        /// </summary>
        /// <param name="openIdSettings">
        /// The OpenId client and server settings. 
        /// This includes the client_id and client_secret, as well as the server endpoints.
        /// </param>
        /// <param name="sessionSessionStateStore">
        /// A session store that will be used to save the state parameters, for CSRF protection.
        /// </param>
        public OpenIdConnectClient(
            OpenIdConnectAuthenticationSettings openIdSettings,
            ISessionStateStore sessionSessionStateStore)
        {
            _openIdSettings = openIdSettings;
            _sessionSessionStateStore = sessionSessionStateStore;
            _codeForAccessTokenExchanger = new HttpClientCodeForAccessTokenExchanger();
        }

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
        public HttpRedirectResponse StartSignIn(string redirectUri)
        {
            return new HttpRedirectResponse(GetIdentityGatewayOicSignInEndpoint(redirectUri));
        }

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
        public HttpRedirectResponse StartSignOut(AccessTokenResponse accessTokenResponse, string redirectUri)
        {
            if (accessTokenResponse == null || accessTokenResponse.IdToken == null)
            {
                // when we have no id_token, like when the user session expires, we cannot sign-out at federation level.
                // in this particular case, we will not redirect to SDB Connect Identity Gateway.
                return new HttpRedirectResponse(redirectUri);
            }

            return new HttpRedirectResponse(GetIdentityGatewayOicSignOutEndpoint(accessTokenResponse, redirectUri));
        }

        /// <summary>
        /// Completes the SignIn flow.
        /// This is the final step in the SignIn flow, that will exchange the authorization code for a access_token.
        /// At this point, the client will perform a "backend" request where it will authenticate itself on the token_endpoint, so the access_token can be generated.
        /// </summary>
        /// <param name="code">The "code" that was provided in the StartSignIn callback.</param>
        /// <param name="state">The "state" that was provided in the StartSignIn callback.</param>
        /// <param name="redirectUri">The original redirect URI that was provided to the StartSignIn method.</param>
        /// <returns>A future AccessContext object containing, among other things, the access_token and the id_token.</returns>
        public async Task<AccessTokenResponse> CompleteSignIn(string code, string state, string redirectUri)
        {
            // retrieve the user's state value and check against the returned state
            var storedState = _sessionSessionStateStore[UserLoginStateKey].ToString();

            if (state != storedState)
            {
                throw new InvalidStateException();
            }

            var accessTokenResponse = await _codeForAccessTokenExchanger.Exchange(_openIdSettings.TokenEndpoint,
                new AccessTokenRequest
                {
                    GrantType = "authorization_code",
                    Code = code,
                    RedirectUri = redirectUri,
                    ClientId = _openIdSettings.ClientId,
                    ClientSecret = _openIdSettings.ClientSecret
                });

            if (string.IsNullOrEmpty(accessTokenResponse.AccessToken))
            {
                throw new Exception("Invalid access token response!");
            }

            return accessTokenResponse;
        }

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
        public async Task<IDictionary<string, string>> GetUserInfo(AccessTokenResponse accessTokenResponse)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _openIdSettings.UserInfoEndpoint);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessTokenResponse.AccessToken);

                // TODO process errors according to the OpenIdConnect standard

                var response = await client.SendAsync(request).ConfigureAwait(false);
                
                var claims = await response
                    .EnsureSuccessStatusCode()
                    .Content
                    .ReadAsAsync<IDictionary<string, string>>()
                    .ConfigureAwait(false);

                return claims;
            }
        }

        /// <summary>
        /// Build the sign in endpoint URI
        /// </summary>
        /// <param name="redirectUri">
        /// The callback URI where the application will call the CompleteSignIn method
        /// </param>
        /// <returns>
        /// The generated URI to the sign in endpoint on SDB Connect IdG.
        /// </returns>
        private Uri GetIdentityGatewayOicSignInEndpoint(string redirectUri)
        {
            // the state is require to protect agaist CSRF attacks.
            // for more information, see http://tools.ietf.org/html/draft-ietf-oauth-v2-26#section-10.12
            // TODO a new GUID is not the correct way of doing this, check the RFC and fix
            var state = Guid.NewGuid().ToString().Replace("-", string.Empty);
            _sessionSessionStateStore[UserLoginStateKey] = state;

            var relativeUrl = string.Format(
                "?state={0}&client_id={1}&response_type=code&scope={2}&redirect_uri={3}",
                state,
                _openIdSettings.ClientId,
                _openIdSettings.Scope,
                redirectUri);

            return new Uri(_openIdSettings.AuthorizationEndpoint, relativeUrl);
        }

        /// <summary>
        /// Build the sign out endpoint URI
        /// </summary>
        /// <param name="accessTokenResponse">
        /// The token endpoint's access token response (when the user completed the sign in flow). 
        /// We need the id_token that was returned in this response.
        /// </param>
        /// <param name="redirectUri">
        /// The callback URI where the application will cleanup the user's session
        /// </param>
        /// <returns>
        /// The generated URI to the sign out endpoint on SDB Connect IdG.
        /// </returns>
        private Uri GetIdentityGatewayOicSignOutEndpoint(AccessTokenResponse accessTokenResponse, string redirectUri)
        {
            var relativeUrl = string.Format(
                "?id_token_hint={0}&post_logout_redirect_uri={1}",
                accessTokenResponse.IdToken,
                redirectUri);

            return new Uri(_openIdSettings.AuthorizationEndpoint, relativeUrl);
        }
    }
}