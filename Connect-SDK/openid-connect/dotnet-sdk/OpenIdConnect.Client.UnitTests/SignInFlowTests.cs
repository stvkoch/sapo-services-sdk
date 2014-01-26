using System;
using System.Threading.Tasks;
using Moq;
using OpenIdConnect.Client.Common;
using OpenIdConnect.Client.Common.Exchangers;
using OpenIdConnect.Client.Common.Representations;
using Xunit;

namespace OpenIdConnect.Client.UnitTests
{
    public class SignInFlowTests
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

            const string signInRedirectUri = "https://dummy.signin.callback.uri";

            var sessionStateStoreMock = new StateSessionStoreMock();
            var accessTokenExchangerMock = new Mock<ICodeForAccessTokenExchanger>();

            var client = new OpenIdConnectClient(config, sessionStateStoreMock, accessTokenExchangerMock.Object);

            var response = client.StartSignIn(signInRedirectUri);

            // now we know that we should have a state in the session store, and that the redirect response should have some things:

            Assert.Equal("https", response.Location.Scheme);
            Assert.Equal("id.services.telecom.pt", response.Location.Host);

            // parameter check

            // do they all exist?
            Assert.True(response.Location.Query.Contains("state"));
            Assert.True(response.Location.Query.Contains("client_id"));
            Assert.True(response.Location.Query.Contains("response_type"));
            Assert.True(response.Location.Query.Contains("scope"));
            Assert.True(response.Location.Query.Contains("redirect_uri"));

            var queryString = TestUtils.GetParsedQueryString(response.Location);

            // are they filled with the correct values?
            Assert.Equal(Uri.EscapeUriString((string)sessionStateStoreMock[OpenIdConnectClient.UserLoginStateKey]), queryString["state"]);
            Assert.Equal(Uri.EscapeUriString(config.ClientId), queryString["client_id"]);
            Assert.Equal(Uri.EscapeUriString("code"), queryString["response_type"]);
            Assert.Equal(Uri.EscapeUriString(config.Scope), queryString["scope"]);
            Assert.Equal(Uri.EscapeUriString(signInRedirectUri), queryString["redirect_uri"]);

            // ensure that the token endpoint is not called on the start of the flow
            accessTokenExchangerMock.Verify(e => e.Exchange(It.IsAny<Uri>(), It.IsAny<AccessTokenRequest>()), Times.Never);
        }

        [Fact]
        public async Task CanCorrectlyCallTokenEndpoint()
        {
            var config = new OpenIdConnectAuthenticationSettings
            {
                ClientId = "some-client-id",
                ClientSecret = "some-client-secret",
                Scope = "openid some-scope",

                SdbIdentityGatewayBaseUri = new Uri("https://id.services.telecom.pt"),
                SdbRuntimeBaseUri = new Uri("https://services.telecom.pt")
            };

            const string signInRedirectUri = "https://dummy.signin.callback.uri";

            var sessionStateStoreMock = new StateSessionStoreMock();
            var accessTokenExchangerMock = new Mock<ICodeForAccessTokenExchanger>();

            // mock setup
            AccessTokenRequest accessTokenRequest = null;
            accessTokenExchangerMock.Setup(e => e.Exchange(It.IsAny<Uri>(), It.IsAny<AccessTokenRequest>()))
            .Returns((Uri baseUri, AccessTokenRequest req) => Task.FromResult(new AccessTokenResponse
            {
                AccessToken = "some-access-token",
                ExpiresIn = "3600",
                IdToken = "the-id-token",
                RefreshToken = "some-refresh-token",
                Scope = config.Scope,
                TokenType = "Bearer"
            }))
            .Callback((Uri baseUri, AccessTokenRequest req) => accessTokenRequest = req);

            var client = new OpenIdConnectClient(config, sessionStateStoreMock, accessTokenExchangerMock.Object);

            // the response itself is not used, but this will create the state in the session state store
            client.StartSignIn(signInRedirectUri);

            // the user is now authenticated and we have an authorization code
            var accessTokenResponse = await client.CompleteSignIn(
                "some-code",
                (string) sessionStateStoreMock[OpenIdConnectClient.UserLoginStateKey],
                signInRedirectUri);

            // ensure that the token endpoint was only called once
            accessTokenExchangerMock.Verify(e => e.Exchange(It.IsAny<Uri>(), It.IsAny<AccessTokenRequest>()), Times.Once);

            // ensure request values
            Assert.Equal(config.ClientId, accessTokenRequest.ClientId);
            Assert.Equal(config.ClientSecret, accessTokenRequest.ClientSecret);
            Assert.Equal("some-code", accessTokenRequest.Code);
            Assert.Equal("authorization_code", accessTokenRequest.GrantType);
            Assert.Equal(signInRedirectUri, accessTokenRequest.RedirectUri);

            // ensure response values

            Assert.Equal("some-access-token", accessTokenResponse.AccessToken);
            Assert.Equal("3600", accessTokenResponse.ExpiresIn);
            Assert.Equal("the-id-token", accessTokenResponse.IdToken);
            Assert.Equal("some-refresh-token", accessTokenResponse.RefreshToken);
            Assert.Equal(config.Scope, accessTokenResponse.Scope);
            Assert.Equal("Bearer", accessTokenResponse.TokenType);
        }
    }
}
