using System.Collections.Generic;

namespace SpotifyLib.DTO.Playlists
{
    public class PlaylistsListResponse
    {
        public string Href { get; set; }
        public IList<PlaylistItem> Items { get; set; }
        public int Total { get; set; }
    }

    public class PlaylistItem
    {
        public bool Collaborative { get; set; }
        public string Description { get; set; }
        public string Href { get; set; }
        public string Id { get; set; }
        public IList<PlaylistItemImage> Images { get; set; }
        public string Name { get; set; }
        public  PlaylistItemTracks Tracks { get; set; }

    }

    public class PlaylistItemImage
    {
        public int Height { get; set; }
        public string Url { get; set; }
        public int Width { get; set; }
    }

    public class PlaylistItemTracks
    {
        public string Href { get; set; }
        public int Total { get; set; }
    }
}
