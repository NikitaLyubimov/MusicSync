using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.Tracks;

namespace SpotifyLib.Interfaces
{
    public interface ITracksClient
    {
        Task<GetTrackResponse> GetTrack(string trackName, string artistName);

    }
}
