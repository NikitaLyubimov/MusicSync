using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SpotifyLib.BaseWeb.DTO;

namespace SpotifyLib.BaseWeb.Interfaces
{
    public interface IAPIConnector
    {
        Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body);
        Task<HttpStatusCode> Post(Uri uri, object body);
        Task<T> Get<T>(Uri uri);
        Task<T> Get<T>(Uri uri, IDictionary<string, string> headers);

    }
}
