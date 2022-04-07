using SpotifyLib.DTO.BaseWeb;
using SpotifyLib.Interfaces.BaseWeb;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.Utils
{
    public class TokenAuthenticator : IAuthenticator
    {
        public TokenAuthenticator(string token)
        {
            Token = token;
        }

        public string Token { get; set; }

        public Task ApplyAsync(Request request, IAPIConnector apiConnector)
        {
            request.Headers["Authorization"] = $"Bearer {Token}";
            return Task.CompletedTask;
        }
    }
}
