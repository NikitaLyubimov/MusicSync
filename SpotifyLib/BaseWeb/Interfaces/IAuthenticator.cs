using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.BaseWeb.Interfaces;
using SpotifyLib.BaseWeb.DTO;

namespace SpotifyLib.BaseWeb.Interfaces
{
    public interface IAuthenticator
    {
        Task Apply(Request request, IAPIConnector apiConnector);
    }
}
