using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpotifyLib.DTO.Autherization
{
    public class AccessTokenRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string GrantType { get;set; }
        public string Code { get; set; }
        public Uri RedirectUri { get; set; }

        public AccessTokenRequest(string clientId, string clientSecret, string grantType, string code, Uri redirectUri)
        {
            ClientId = clientId;
            ClientSecret = clientSecret;
            GrantType = grantType;
            Code = code;
            RedirectUri = redirectUri;
        }
    }
}
