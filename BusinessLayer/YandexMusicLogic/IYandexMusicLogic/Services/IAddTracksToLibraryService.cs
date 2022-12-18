using CoreLib.TracksDTOs;
using IYandexMusicLogic.DTOs.Response;

namespace IYandexMusicLogic.Services
{
    public interface IAddTracksToLibraryService
    {
        Task<AddTracksResponse> AddTracksToLibrary(TracksForQueueDto addTracksRequest);
    }
}
