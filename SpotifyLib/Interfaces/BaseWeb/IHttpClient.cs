using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.BaseWeb;

namespace SpotifyLib.Interfaces.BaseWeb
{
    public interface IHttpClient
    {
        Task<Response> DoRequest(Request request);
        IEnumerable<Cookie> GetCookies(Uri uri);
    }
}
