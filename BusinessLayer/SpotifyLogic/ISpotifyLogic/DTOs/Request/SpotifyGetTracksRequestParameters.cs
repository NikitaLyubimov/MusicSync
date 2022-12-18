using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ISpotifyLogic.ViewModels.Request
{
    public class SpotifyGetTracksRequestParameters
    {
        public int Offset { get; set; }
        public int Limit { get; set; }
    }
}
