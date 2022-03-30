using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.DTOs.Response
{
    public class TracksResponse
    {
        public IList<TrackDtoResponse> Tracks { get; set; }
    }

    public class TrackDtoResponse
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
    }
}
