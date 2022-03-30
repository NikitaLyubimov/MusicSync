using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using BaseWeb.DTO;

namespace BaseWeb.Interfaces
{
    public delegate Task AuthenticatorHandlerAsync(Request request, IAPIConnector apiConnector);
    public delegate void AuthenticatorHandler(Request request, IAPIConnector apiConnector);
    public interface IAPIConnector
    {
        Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body);
        Task<T> Post<T>(Uri uri, object body);
        Task<Response> Post(Uri uri, object body);
        Task<Response> Put(Uri uri);
        Task<T> Get<T>(Uri uri);
        Task<Response> Get(Uri uri);
        Task<T> Get<T>(Uri uri, IDictionary<string, string> headers);
        IEnumerable<Cookie> GetRequestCookies(Uri uri);

        bool SetAuthenticator(IAuthenticator authenticator);

    }
}
