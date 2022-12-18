using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Music.Api.Models.Track;
using YandexMusicService.DTOs.Response;
using YandexMusicService.Utils.Interfaces;

namespace YandexMusicService.Utils.Implementations
{
    public class RetryHandler : IRetryHandler
    {
        public async Task<AddTrackResponse> HandleRetry(IRetryHandler.RetryMethod retry)
        {
            return await HandleRetryInternal(retry);
        }

        private async Task<AddTrackResponse> HandleRetryInternal(IRetryHandler.RetryMethod retry)
        {
            try
            {
                await Task.Delay(1000);
                var result = await retry().ConfigureAwait(false);
                return result;
            }
            catch (Exception ex)
            {
                return await HandleRetryInternal(retry);
            }
        }
    }
}
