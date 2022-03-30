using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.ViewModels.Request
{
    public class SpotifyGetTracksRequestParameters
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
