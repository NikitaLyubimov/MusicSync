using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Playlists
{
    public class AddTracksToPlaylistRequest
    {
        public string PlaylistId { get; set; }
        public IEnumerable<string> Tracks { get; set; }
    }
}
