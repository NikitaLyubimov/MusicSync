using System;
using System.Net;
using System.Threading.Tasks;


using SpotifyLib.BaseWeb.Interfaces;
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
        public async Task<HttpStatusCode> AddTracksToPlaylist(AddTracksToPlaylistRequest addTracksRequest, string playlistId)
        {
            return await _apiConnector.Post(SpotifyUrls.AddItemsToPlaylist(playlistId), addTracksRequest);
        }

        public async Task<HttpStatusCode> CreatePlaylist(CreatePlaylistRequest createPlaylistRequest)
        {
            return await _apiConnector.Post(SpotifyUrls.CreatePlaylistUri("9itg0t81brz9r8xr557ax329e"), createPlaylistRequest);
        }

        public async Task<PlaylistsListResponse> GetCurrentUserPlaylists()
        {
            return await SendCurrentUserPlaylistsRequest<PlaylistsListResponse>(_apiConnector);
        }

        public static async Task<T> SendCurrentUserPlaylistsRequest<T>(IAPIConnector apiConnector)
        {
            return await apiConnector.Get<T>(SpotifyUrls.GetPlaylistsUri);
        }

        public async Task<PlaylistTracksResponse> GetPlaylistTracks(string playlistId)
        {
            var getPlaylistTracksPath = $"{SpotifyUrls.PlaylistUri}/{playlistId}/tracks";
            var getPlaylistTracksUri = new Uri(getPlaylistTracksPath);
            return await _apiConnector.Get<PlaylistTracksResponse>(getPlaylistTracksUri);
        }
        
    }
}
