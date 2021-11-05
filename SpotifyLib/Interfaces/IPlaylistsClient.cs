using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaseWeb.DTO;
using SpotifyLib.DTO.Playlists;

namespace SpotifyLib.Interfaces
{
    public interface IPlaylistsClient
    {
        Task<PlaylistsListResponse> GetCurrentUserPlaylists();
        Task<PlaylistTracksResponse> GetPlaylistTracks(string playlistId);
        Task<Response> CreatePlaylist(CreatePlaylistRequest createPlaylistRequest);
        Task<Response> AddTracksToPlaylist(AddTracksToPlaylistRequest addTracksRequest, string playlistId);
    }
}
