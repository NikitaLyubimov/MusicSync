using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YandexMusicService.DTOs.Request;
using YandexMusicService.DTOs.Response;

using CoreLib.TracksDTOs;

namespace YandexMusicService.Services.Interfaces
{
    public interface IAddTracksToLibraryService
    {
        Task<AddTracksResponse> AddTracksToLibrary(TracksForQueueDto addTracksRequest);
    }
}
