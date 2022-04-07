using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.BaseWeb;

namespace SpotifyLib.Interfaces.BaseWeb
{
    public interface IJSONSerializer
    {
        void SerializeRequest(Request request);
        ApiResponse<T> DeserealizeResponse<T>(Response response);
    }
}
