using SpotifyLib.DTO.BaseWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Interfaces
{
    public interface IRetryHandler
    {
        delegate Task<Response> RetryMethod(Request request);
        Task<Response> HandleRetry(Request request, Response response, RetryMethod retry);
    }
}
