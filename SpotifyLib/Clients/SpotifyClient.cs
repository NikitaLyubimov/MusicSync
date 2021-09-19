using SpotifyLib.BaseWeb.Implementation;
using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.Constants;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Clients
{
    public class SpotifyClient
    {
        public IPlaylistsClient Playlists { get; set; }
        public IAuthenticationClient Authentication { get; set; }

        private IAPIConnector _apiConnector;

        public SpotifyClient(AccessTokenResponse accessToken, string clientId, string clientSecret)
        {
            var authenticator = new Authenticator(clientId, clientSecret, accessToken);
            _apiConnector = new APIConnector(SpotifyUrls.SpotifyApiUri, authenticator, new JSONSerializer(), new NetHttpClient());

            Playlists = new PlaylistsClient(_apiConnector);
            Authentication = new AuthenticationClient(_apiConnector);
        }
    }
}
