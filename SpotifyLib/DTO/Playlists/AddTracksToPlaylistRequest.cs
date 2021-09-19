using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Playlists
{
    public class AddTracksToPlaylistRequest
    {
        public IEnumerable<string> Uris { get; set; }
    }
}
