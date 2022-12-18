using System.Threading.Tasks;

using YandexMusicService.DTOs.Response;
using CoreLib.Playlists;

namespace YandexMusicService.Services.Interfaces
{
    public interface IAddPlaylistsToLibraryService
    {
        Task<AddPlaylistsResponse> AddPlaylistsToLibrary(PlaylistsForQueueDto playlistsForQueue);
    }
}
