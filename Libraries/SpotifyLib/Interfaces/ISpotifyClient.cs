using SpotifyLib.DTO.Autherization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Interfaces
{
    public interface ISpotifyClient
    {
        IPlaylistsClient PlaylistsClient { get;  }
        IAuthenticationClient AuthenticationClient { get; }
        ITracksClient TracksClient { get; }
        Task<bool> Auth(string clientId, string clientSecret, string code, Uri returnUri);
    }
}
