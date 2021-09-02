using System.Threading.Tasks;

using SpotifyLib.BaseWeb.DTO;
using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Clients;

namespace SpotifyLib.BaseWeb.Implementation
{
    public class Authenticator : IAuthenticator
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public AccessTokenResponse AccessToken { get; set; }

        public Authenticator(string clientId, string clientSecret, AccessTokenResponse accessToken)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            AccessToken = accessToken;
        }
        public async Task Apply(Request request, IAPIConnector apiConnector)
        {
            if (AccessToken.IsExpired)
            {
                var tokenRequest = new RefreshTokenRequest(ClientId, ClientSecret, "refresh_token", AccessToken.RefreshToken);
                var refreshToken = await AuthenticationClient.SendRefreshTokenRequest(tokenRequest, apiConnector).ConfigureAwait(false);

                AccessToken.Token = refreshToken.Token;
                AccessToken.RefreshToken = refreshToken.RefreshToken;
                AccessToken.Scope = refreshToken.Scope;
                AccessToken.TokenType = refreshToken.TokenType;
                AccessToken.ExpiresIn = refreshToken.ExpiresIn;
                AccessToken.CreatedAt = refreshToken.CreatedAt;

            }

            request.Headers["Authorization"] = $"{AccessToken.TokenType} {AccessToken.Token}";
        }
    }
}
