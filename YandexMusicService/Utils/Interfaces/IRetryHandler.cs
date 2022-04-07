using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Music.Api.Models.Track;
using YandexMusicService.DTOs.Response;

namespace YandexMusicService.Utils.Interfaces
{
    public interface IRetryHandler
    {
        delegate Task<AddTrackResponse> RetryMethod();
        Task<AddTrackResponse> HandleRetry(RetryMethod retry);
    }
}
