using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YandexMusicService.DTOs.Request
{
    public class AddTracksRequest
    {
        public IList<TrackForAdd> Tracks { get; set; }
    }

    public class TrackForAdd
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
    }
}
