using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SpotifyLib.Clients;

using SpotifyService.ViewModels;
using SpotifyService.ViewModels.Request;
using SpotifyLib.DTO.Autherization;
using SpotifyLib.Interfaces;

namespace SpotifyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyTracksController : ControllerBase
    {
        private readonly SpotifyCredsViewModel _spotifyCreds;
        private readonly ISpotifyClient _spotifyClient;
        public SpotifyTracksController(IOptions<SpotifyCredsViewModel> spotifyCreds, ISpotifyClient spotifyClient)
        {
            _spotifyCreds = spotifyCreds.Value;
            _spotifyClient = spotifyClient;
        }

        [HttpGet("GetTracks")]
        public async Task<IActionResult> GetTracks([FromQuery]SpotifyGetTracksRequestParameters parameters)
        {
            var result = await _spotifyClient.TracksClient.GetTracks(parameters.Offset, parameters.Limit);
            return Ok(result);
        }

        [HttpGet("GetTrack")]
        public async Task<IActionResult> GetTrack([FromQuery]SpotifyGetTrackRequestParameters parameters)
        {
            var result = await _spotifyClient.TracksClient.GetTrack(parameters.TrackName, parameters.ArtistName);
            return Ok(result);
        }
    }
}
