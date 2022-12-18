using System.Threading.Tasks;
using System.Collections.Generic;

using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.Interfaces.BaseWeb;
using SpotifyLib.Constants;
using SpotifyLib.DTO.Playlists;
using SpotifyLib.Interfaces;

namespace SpotifyLib.Clients
{
    public class PlaylistsClient : IPlaylistsClient
    {
        private IAPIConnector _apiConnector;

        public PlaylistsClient(IAPIConnector apiConnector)
        {
            _apiConnector = apiConnector;
        }
        public async Task<Response> AddTracksToPlaylist(AddTracksToPlaylistRequest addTracksRequest, string playlistId)
        {
            return await _apiConnector.Post(SpotifyUrls.AddItemsToPlaylist(playlistId), addTracksRequest);
        }

        public async Task<Response> CreatePlaylist(CreatePlaylistRequest createPlaylistRequest)
        {
            return await _apiConnector.Post(SpotifyUrls.CreatePlaylistUri("9itg0t81brz9r8xr557ax329e"), createPlaylistRequest);
        }

        public async Task<PlaylistsListResponse> GetCurrentUserPlaylists(int limit, int offset)
        {
            if(limit == 0 && offset == 0)
                return await _apiConnector.Get<PlaylistsListResponse>(SpotifyUrls.GetPlaylistsUri);
            else
            {
                var parameters = new Dictionary<string, string>
                {
                    {"limit", limit.ToString() },
                    {"offset", offset.ToString() }
                };
                return await _apiConnector.Get<PlaylistsListResponse>(SpotifyUrls.GetPlaylistsUri, null, parameters);
            }
        }

        public async Task<PlaylistTracksResponse> GetPlaylistTracks(string playlistId, int limit = 0, int offset = 0)
        {
            if (limit == 0 && offset == 0)
                return await _apiConnector.Get<PlaylistTracksResponse>(SpotifyUrls.GetPlaylistTracksUri(playlistId));
            else
            {
                var parameters = new Dictionary<string, string>
                {
                    {"limit", limit.ToString() },
                    {"offset", offset.ToString() }
                };
                return await _apiConnector.Get<PlaylistTracksResponse>(SpotifyUrls.GetPlaylistTracksUri(playlistId), null, parameters);
            }
        }
        
    }
}
