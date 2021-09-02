using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Playlists
{
    public class CreatePlaylist
    {
        public string Name { get; set; }
        public bool Public { get; set; }
        public bool Collaborative { get; set; }
        public string Description { get; set; }
    }
}
