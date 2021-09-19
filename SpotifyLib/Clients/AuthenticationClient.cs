using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

using SpotifyLib.DTO.Autherization;
using SpotifyLib.Interfaces;
using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.Constants;
using SpotifyLib.BaseWeb.Implementation;

namespace SpotifyLib.Clients
{
    public class AuthenticationClient : IAuthenticationClient
    {
        private IAPIConnector _apiConnector;

        public AuthenticationClient(IAPIConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }

        public AuthenticationClient()
        {
            _apiConnector = new APIConnector(SpotifyUrls.AuthTokenRequest, null, new JSONSerializer(), new NetHttpClient());
        }
        public async Task<AccessTokenResponse> RequestRefreshToken(RefreshTokenRequest refreshTokenRequest)
        {
            return await SendRefreshTokenRequest(refreshTokenRequest, _apiConnector);
        }

        public async Task<AccessTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest)
        {
            return await SendTokenRequest(accessTokenRequest, _apiConnector);
        }

        public static async Task<AccessTokenResponse> SendTokenRequest(AccessTokenRequest accessTokenRequest, IAPIConnector apiConnector)
        {
            var form = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", accessTokenRequest.GrantType),
                new KeyValuePair<string, string>("code", accessTokenRequest.Code),
                new KeyValuePair<string, string>("redirect_uri", accessTokenRequest.RedirectUri.ToString())
            };

            return await SendAuthRequest<AccessTokenResponse>(apiConnector, form, accessTokenRequest.ClientId, accessTokenRequest.ClientSecret);

        }

        public static async Task<AccessTokenResponse> SendRefreshTokenRequest(RefreshTokenRequest refreshTokenRequest, IAPIConnector apiConnector)
        {
            var form = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("grant_type", refreshTokenRequest.GrantType),
                new KeyValuePair<string, string>("refresh_token", refreshTokenRequest.RefreshToken)
            };

            return await SendAuthRequest<AccessTokenResponse>(apiConnector, form, refreshTokenRequest.ClientId, refreshTokenRequest.ClientSecret);
        }

        private static async Task<T> SendAuthRequest<T>(IAPIConnector apiConnector, List<KeyValuePair<string, string>> form, 
            string clientId, string clientSecret)
        {
            var header = BuildHeader(clientId, clientSecret);
            return await apiConnector.Post<T>(SpotifyUrls.AuthTokenRequest, header, new FormUrlEncodedContent(form));
        }

        private static Dictionary<string, string> BuildHeader(string clientId, string clientSecret)
        {
            if (string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(clientSecret))
                return new Dictionary<string, string>();

            var authBase64 = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}"));
            var authValue = string.Concat("Basic ", authBase64);

            return new Dictionary<string, string>
            {
                {"Authorization", authValue }
            };
        }
    }
}
