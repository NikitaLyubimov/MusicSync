using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.DTOs.Response
{
    public class PlaylistsForQueueResponse
    {
        public List<PlaylistForQueue> Playlists { get; set; }
    }

    public class PlaylistForQueue
    {
        public string SpotifyId { get; set; }
        public string Name { get; set; }
        public IList<TrackDtoResponse> Tracks { get; set; }
    }
}
