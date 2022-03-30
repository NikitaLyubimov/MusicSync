using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YandexMusicService.DTOs.Response
{
    public class AddTracksResponse
    {
        public IList<AddTrackResponse> Tracks { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionString { get; set; }
    }

    public class AddTrackResponse
    {
        public string Id { get; set; }
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public bool IsSuccessAdded { get; set; }
    }
}
