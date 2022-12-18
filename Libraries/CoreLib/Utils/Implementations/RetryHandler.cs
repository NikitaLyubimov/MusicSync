using CoreLib.Utils.Interfaces;
using System;

using System.Threading.Tasks;


namespace CoreLib.Utils.Implementations
{
    public class RetryHandler : IRetryHandler
    {
        public async Task<TResponse> HandleRetry<TResponse>(IRetryHandler.RetryMethod<TResponse> retry)
        {
            return await HandleRetryInternal<TResponse>(retry);
        }

        private async Task<TResponse> HandleRetryInternal<TResponse>(IRetryHandler.RetryMethod<TResponse> retry)
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
