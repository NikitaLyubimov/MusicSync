using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.ViewModels
{
    public class SpotifyCredsViewModel
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string ReturnUri { get; set; }
    }
}
