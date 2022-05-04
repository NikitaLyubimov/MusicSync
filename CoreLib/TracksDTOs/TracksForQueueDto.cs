using System.Collections.Generic;

namespace CoreLib.TracksDTOs
{
    public class TracksForQueueDto
    {
        public IList<TrackDtoResponse> Tracks { get; set; }
    }

    public class TrackDtoResponse
    {
        public string ArtistName { get; set; }
        public string TrackName { get; set; }
    }
}
