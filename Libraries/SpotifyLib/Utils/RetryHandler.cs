using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Utils
{
    public class RetryHandler : IRetryHandler
    {
        public TimeSpan RetryAfter { get; set; }
        public int RetryTimes { get; set; }
        public bool TooManyRequestsConsumesARetry { get; set; }


        public RetryHandler()
        {
            RetryTimes = 10;
            TooManyRequestsConsumesARetry = false;
            RetryAfter = TimeSpan.FromMilliseconds(1000);
        }

        private TimeSpan? GetRetryAfterValue(Response response)
        {
            if (response.StatusCode != (HttpStatusCode)429)
            {
                TooManyRequestsConsumesARetry = false;
                return null;
            }
                
            if (response.Headers.ContainsKey("Retry-After") && int.TryParse(response.Headers["Retry-After"], out int retryAfterValue)
                || response.Headers.ContainsKey("retry-after") && int.TryParse(response.Headers["retry-after"], out retryAfterValue))
            {
                TooManyRequestsConsumesARetry = true;
                return TimeSpan.FromSeconds(retryAfterValue);
            }
                

            throw new Exception("Couldn't find retry after header");
        }

        public async Task<Response> HandleRetry(Request request, Response response, IRetryHandler.RetryMethod retry)
        {
            return await HandleRetryInternal(request, response, retry, RetryTimes);
        }
        private bool IsSuccess(Response response)
        {
            return response.StatusCode == HttpStatusCode.OK;
        }
        private async Task<Response> HandleRetryInternal(Request request, Response response, IRetryHandler.RetryMethod retry, int triesLeft)
        {
            var retryAfterSeconds = GetRetryAfterValue(response);
            if (retryAfterSeconds != null && (!TooManyRequestsConsumesARetry || triesLeft > 0))
            {
                await Task.Delay(retryAfterSeconds.Value).ConfigureAwait(false);
                response = await retry(request).ConfigureAwait(false);
                if (IsSuccess(response))
                    return response;
                var newTriesLeft = TooManyRequestsConsumesARetry ? triesLeft - 1 : triesLeft;
                return await HandleRetryInternal(request, response, retry, newTriesLeft).ConfigureAwait(false);

            }
            return response;
        }
    }
}
