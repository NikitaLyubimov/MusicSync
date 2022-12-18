using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.DTO.Playlists;

namespace SpotifyLib.Interfaces
{
    public interface IPlaylistsClient
    {
        Task<PlaylistsListResponse> GetCurrentUserPlaylists(int limit = 0, int offset = 0);
        Task<PlaylistTracksResponse> GetPlaylistTracks(string playlistId, int limit = 0, int offset = 0);
        Task<Response> CreatePlaylist(CreatePlaylistRequest createPlaylistRequest);
        Task<Response> AddTracksToPlaylist(AddTracksToPlaylistRequest addTracksRequest, string playlistId);
    }
}
