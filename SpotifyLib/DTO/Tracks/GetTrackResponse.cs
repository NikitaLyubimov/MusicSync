﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Tracks
{
    public class GetTrackResponse
    {
        public IList<TrackArtist> Artists { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }

    }

    public class TrackAlbum
    {
        public IList<TrackArtist> Artists { get; set; }
        public string Name { get; set; }
    }

    public class TrackArtist
    {
        public string Name { get; set; }
        public string Uri { get; set; }
    }


}
