using CoreLib.Utils.Interfaces;
using System;

using System.Threading.Tasks;


namespace CoreLib.Utils.Implementations
{
    public class RetryHandler<T> : IRetryHandler<T>
    {
        public async Task<T> HandleRetry(IRetryHandler<T>.RetryMethod retry)
        {
            return await HandleRetryInternal(retry);
        }

        private async Task<T> HandleRetryInternal(IRetryHandler<T>.RetryMethod retry)
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
