using System;
using System.Collections.Generic;
using System.Text;

using CoreLib.TracksDTOs;

namespace CoreLib.Playlists
{
    public class PlaylistsForQueueDto
    {
        public List<PlaylistForQueue> Playlists { get; set; }
    }
    public class PlaylistForQueue
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public IList<TrackDtoResponse> Tracks { get; set; }
    }
}
