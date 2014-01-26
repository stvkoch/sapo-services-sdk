using System;
using OpenIdConnect.Client.Common;
using OpenIdConnect.Client.Common.Representations;
using Xunit;

namespace OpenIdConnect.Client.UnitTests
{
    public class SignOutFlowTests
    {
        [Fact]
        public void CanRedirectToCorrectEndpoint()
        {
            var config = new OpenIdConnectAuthenticationSettings
            {
                ClientId = "some-client-id",
                ClientSecret = "some-client-secret",
                Scope = "openid some-scope",

                SdbIdentityGatewayBaseUri = new Uri("https://id.services.telecom.pt"),
                SdbRuntimeBaseUri = new Uri("https://services.telecom.pt")
            };

            const string signOutRedirectUri = "https://dummy.signout.callback.uri";

            var sessionStateStoreMock = new StateSessionStoreMock();
            var client = new OpenIdConnectClient(config, sessionStateStoreMock);

            var response = client.StartSignOut(new AccessTokenResponse { IdToken = "the-id-token" }, signOutRedirectUri);

            // parameter check

            // do they all exist?
            Assert.True(response.Location.Query.Contains("id_token_hint"));
            Assert.True(response.Location.Query.Contains("post_logout_redirect_uri"));

            var queryString = TestUtils.GetParsedQueryString(response.Location);

            // are they filled with the correct values?
            Assert.Equal(Uri.EscapeUriString(signOutRedirectUri), queryString["post_logout_redirect_uri"]);
            Assert.Equal(Uri.EscapeUriString("the-id-token"), queryString["id_token_hint"]);
        }

        [Fact]
        public void RedirectsToRedirectUriImmediatellyWithoutIdToken()
        {
            var config = new OpenIdConnectAuthenticationSettings
            {
                ClientId = "some-client-id",
                ClientSecret = "some-client-secret",
                Scope = "openid some-scope",

                SdbIdentityGatewayBaseUri = new Uri("https://id.services.telecom.pt"),
                SdbRuntimeBaseUri = new Uri("https://services.telecom.pt")
            };

            const string signOutRedirectUri = "https://dummy.signout.callback.uri";

            var sessionStateStoreMock = new StateSessionStoreMock();
            var client = new OpenIdConnectClient(config, sessionStateStoreMock);

            var response = client.StartSignOut(null, signOutRedirectUri);

            Assert.Equal(signOutRedirectUri, response.Location.OriginalString);
        }
    }
}
