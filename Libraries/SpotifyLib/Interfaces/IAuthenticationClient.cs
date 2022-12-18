using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SpotifyLib.DTO.Autherization;

namespace SpotifyLib.Interfaces
{
    public interface IAuthenticationClient
    {
        Task<AccessTokenResponse> RequestToken(AccessTokenRequest accessTokenRequest);
        Task<AccessTokenResponse> RequestRefreshToken(RefreshTokenRequest refreshTokenRequest);
    }
}
