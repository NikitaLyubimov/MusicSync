using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseWeb.Implementation;
using BaseWeb.Interfaces;
using SpotifyLib.Constants;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Interfaces;
using SpotifyLib.Utils;

namespace SpotifyLib.Clients
{
    public class SpotifyClient
    {
        public IPlaylistsClient Playlists { get; set; }
        public IAuthenticationClient Authentication { get; set; }
        public ITracksClient Tracks { get; set; }
        private IAPIConnector _apiConnector;

        public SpotifyClient(AccessTokenResponse accessToken, string clientId, string clientSecret)
        {
            var authenticator = new Authenticator(clientId, clientSecret, accessToken);
            _apiConnector = new APIConnector(SpotifyUrls.SpotifyApiUri, new JSONSerializer(), new NetHttpClient());
            _apiConnector.AuthenticatorNotifyAsync += authenticator.ApplyAsync;

            Playlists = new PlaylistsClient(_apiConnector);
            Authentication = new AuthenticationClient(_apiConnector);
            Tracks = new TracksClient(_apiConnector);
        }
    }
}
