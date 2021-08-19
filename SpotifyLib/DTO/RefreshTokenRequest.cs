using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO
{
    public class RefreshTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get; set; }
        public string RefreshToken { get; set; }

        public RefreshTokenRequest(string clientId, string clientSecret, string grantType, string refreshToken)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            GrantType = grantType;
            RefreshToken = refreshToken;
        }
    }
}
