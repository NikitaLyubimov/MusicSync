using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SpotifyService.DTOs.Response;

namespace SpotifyService.Services.Interfaces
{
    public interface ISynchroniseTracksService
    {
        Task<bool> SynchroniseTracks(TracksResponse tracks);
    }
}
