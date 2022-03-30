using System.Threading.Tasks;

using BaseWeb.DTO;
using SpotifyLib.Interfaces;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Clients;
using BaseWeb.Interfaces;

namespace SpotifyLib.Utils
{
    public class Authenticator : IAuthenticator
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public AccessTokenResponse AccessToken { get; set; }

        public Authenticator(string clientId, string clientSecret, AccessTokenResponse accessToken = null)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            if (accessToken == null)
                AccessToken = new AccessTokenResponse();
            else
                AccessToken = accessToken;
        }
        public async Task ApplyAsync(Request request, IAPIConnector apiConnector)
        {
            if (AccessToken.IsExpired)
            {
                var tokenRequest = new RefreshTokenRequest(ClientId, ClientSecret, "refresh_token", AccessToken.RefreshToken);
                var refreshToken = await AuthenticationClient.SendRefreshTokenRequest(tokenRequest, apiConnector).ConfigureAwait(false);

                AccessToken.AccessToken = refreshToken.AccessToken;
                AccessToken.RefreshToken = refreshToken.RefreshToken;
                AccessToken.Scope = refreshToken.Scope;
                AccessToken.TokenType = refreshToken.TokenType;
                AccessToken.ExpiresIn = refreshToken.ExpiresIn;
                AccessToken.CreatedAt = refreshToken.CreatedAt;

            }

            request.Headers["Authorization"] = $"{AccessToken.TokenType} {AccessToken.AccessToken}";
        }

    }
}
