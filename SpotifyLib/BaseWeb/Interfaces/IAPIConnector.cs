using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SpotifyLib.BaseWeb.DTO;

namespace SpotifyLib.BaseWeb.Interfaces
{
    public interface IAPIConnector
    {
        Task<T> Post<T>(Uri uri, IDictionary<string, string> headers, object body);
    }
}
