using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using ISpotifyLogic.Services;
using ISpotifyLogic.ViewModels.Request;
using ISpotifyLogic.DTOs.Response;

namespace SpotifyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyTracksController : ControllerBase
    {
        private readonly IPushTracksToSyncQueueService _pushTracksToSyncQueueService;
        private readonly IGetTracksService _getTracksService;
        public SpotifyTracksController(IPushTracksToSyncQueueService pushTracksToSyncQueueService, IGetTracksService getTracksService)
        {
            _pushTracksToSyncQueueService = pushTracksToSyncQueueService;
            _getTracksService = getTracksService;
        }

        [HttpGet("GetTracks")]
        public async Task<IActionResult> GetTracks([FromQuery]SpotifyGetTracksRequestParameters parameters)
        {
            try
            {
                var result = await _getTracksService.GetTracks(parameters);
                return Ok(result);
            }
            catch
            {
                return BadRequest(new TracksMetadataResponse { ExceptionString = "Unknown error occured" });
            }
        }

        [HttpPost("AddTracksInSyncQueue")]
        public async Task<IActionResult> AddTracksInSyncQueue([FromQuery]string queueType)
        {
            try
            {
                await _pushTracksToSyncQueueService.PushTracks(queueType);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
