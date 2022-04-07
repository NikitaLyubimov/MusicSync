using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using YandexMusicService.DTOs.Request;
using YandexMusicService.DTOs.Response;

namespace YandexMusicService.Services.Interfaces
{
    public interface IAddTracksToLibraryService
    {
        Task<AddTracksResponse> AddTracksToLibrary(AddTracksRequest addTracksRequest);
    }
}
