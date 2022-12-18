using System.Threading.Tasks;

namespace CoreLib.Utils.Interfaces
{
    public interface IRetryHandler<T>
    {
        delegate Task<T> RetryMethod();
        Task<T> HandleRetry(RetryMethod retry);
    }
}
