using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SpotifyLib.Interfaces;
using SpotifyService.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyUserAuthenticationController : ControllerBase
    {
        private readonly SpotifyCredsViewModel _spotifyCreds;
        private readonly ISpotifyClient _spotifyClient;

        public SpotifyUserAuthenticationController(ISpotifyClient spotifyClient, IOptions<SpotifyCredsViewModel> spotifyCreds)
        {
            _spotifyClient = spotifyClient;
            _spotifyCreds = spotifyCreds.Value;
        }

        [HttpGet("Auth")]
        public async Task<IActionResult> Auth(string code)
        {
            try
            {
                await _spotifyClient.Auth(_spotifyCreds.ClientId, _spotifyCreds.ClientSecret, code, new Uri(_spotifyCreds.ReturnUri));
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
