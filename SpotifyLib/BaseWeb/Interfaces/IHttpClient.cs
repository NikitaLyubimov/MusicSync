using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.BaseWeb.DTO;

namespace SpotifyLib.BaseWeb.Interfaces
{
    public interface IHttpClient
    {
        Task<Response> DoRequest(Request request);
    }
}
