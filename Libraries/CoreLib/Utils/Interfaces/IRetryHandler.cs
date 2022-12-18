using System.Threading.Tasks;

namespace CoreLib.Utils.Interfaces
{
    public interface IRetryHandler
    {
        delegate Task<TResponse> RetryMethod<TResponse>();
        Task<TResponse> HandleRetry<TResponse>(RetryMethod<TResponse> retry);
    }
}
