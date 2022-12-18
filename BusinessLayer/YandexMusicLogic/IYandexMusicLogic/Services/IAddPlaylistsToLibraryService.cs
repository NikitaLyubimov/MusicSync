using CoreLib.Playlists;
using IYandexMusicLogic.DTOs.Response;

namespace IYandexMusicLogic.Services
{
    public interface IAddPlaylistsToLibraryService
    {
        Task<AddPlaylistsResponse> AddPlaylistsToLibrary(PlaylistsForQueueDto playlistsForQueue);
    }
}
