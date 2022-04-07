using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.BaseWeb;

namespace SpotifyLib.Interfaces.BaseWeb
{
    public interface IAuthenticator
    {
        Task ApplyAsync(Request request, IAPIConnector apiConnector);
    }
}
