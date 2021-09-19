using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.Playlists;

namespace SpotifyLib.Interfaces
{
    public interface IPlaylistsClient
    {
        Task<PlaylistsListResponse> GetCurrentUserPlaylists();
        Task<PlaylistTracksResponse> GetPlaylistTracks(string playlistId);
        Task<HttpStatusCode> CreatePlaylist(CreatePlaylistRequest createPlaylistRequest);
        Task<HttpStatusCode> AddTracksToPlaylist(AddTracksToPlaylistRequest addTracksRequest, string playlistId);
    }
}
