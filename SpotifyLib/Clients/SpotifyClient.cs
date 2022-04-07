using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.BaseWebImplementation;
using SpotifyLib.Interfaces.BaseWeb;
using SpotifyLib.Constants;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Interfaces;
using SpotifyLib.Utils;

namespace SpotifyLib.Clients
{
    public class SpotifyClient : ISpotifyClient
    {
        private IPlaylistsClient _playlists { get; set; }
        private IAuthenticationClient _authentication { get; set; }
        private ITracksClient _tracks { get; set; }

        IPlaylistsClient ISpotifyClient.PlaylistsClient => _playlists;

        IAuthenticationClient ISpotifyClient.AuthenticationClient => _authentication;

        ITracksClient ISpotifyClient.TracksClient => _tracks;

        private IAPIConnector _apiConnector;

        public SpotifyClient(string clientId, string clientSecret, AccessTokenResponse accessToken = null)
        {
            IAuthenticator authenticator = null;
            if(accessToken != null)
                authenticator =  new Authenticator(clientId, clientSecret, accessToken);
            _apiConnector = new APIConnector(SpotifyUrls.SpotifyApiUri, new JSONSerializer(), new NetHttpClient(), authenticator);

        }

        public async Task<bool> Auth(string clientId, string clientSecret, string code, Uri returnUri)
        {
            var tokenRequestObj = new AccessTokenRequest(clientId, clientSecret, "authorization_code", code, returnUri);
            var response = await new AuthenticationClient().RequestToken(tokenRequestObj);
            var authenticator = new Authenticator(clientId, clientSecret, response);
            _apiConnector.SetAuthenticator(authenticator);

            _playlists = new PlaylistsClient(_apiConnector);
            _authentication = new AuthenticationClient(_apiConnector);
            _tracks = new TracksClient(_apiConnector);

            return true;
        }
    }
}
