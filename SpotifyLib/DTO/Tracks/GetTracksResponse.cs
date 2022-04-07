using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Tracks
{
    public class GetTracksResponse
    {
        public IList<UserItemInfo> Items { get; set; }
        public int Total { get; set; }
    }

    public class UserItemInfo
    {
        public string AddedAt { get; set; }
        public UserTrackInfo Track { get; set; }


    }

    public class UserTrackInfo
    {
        public AlbumInfo Album { get; set; }
        public IList<ArtistInfo> Artists { get; set; }
        public string Name { get; set; }
        public string Uri { get; set; }
    }

    public class AlbumInfo
    {
        public string Href { get; set; }
        public string Id { get; set; }
        public IList<ImageInfo> Images { get; set; }
        public string Name { get; set; }
        public string ReleaseDate { get; set; }
        public string Uri { get; set; }

    }

    public class ArtistInfo
    {
        public string Href { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public string UrI { get; set; }
    }

    public class ImageInfo
    {
        public string Height { get; set; }
        public string Url { get; set; }
        public string Width { get; set; }
    }
}
