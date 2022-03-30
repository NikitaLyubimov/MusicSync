using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.ViewModels.Request
{
    public class SpotifyGetTrackRequestParameters
    {
        public string TrackName { get; set; }
        public string ArtistName { get; set; }
    }
}
