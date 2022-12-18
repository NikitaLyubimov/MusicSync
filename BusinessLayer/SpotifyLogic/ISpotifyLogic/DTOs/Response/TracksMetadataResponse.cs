using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISpotifyLogic.DTOs.Response
{
    public class TracksMetadataResponse
    {
        public IList<TrackMetadataResponse> Tracks { get; set; }
        public string ExceptionString { get; set; }
    }

    public class TrackMetadataResponse
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
        public string AlbumName { get; set; }
        public IList<TrackImageInfo> TrackImages { get; set; } 
    }

    public class TrackImageInfo
    {
        public string Height { get; set; }
        public string Uri { get; set; }
        public string Width { get; set; }
    }
}
