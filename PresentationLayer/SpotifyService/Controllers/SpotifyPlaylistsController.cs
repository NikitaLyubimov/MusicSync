using ISpotifyLogic.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace SpotifyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpotifyPlaylistsController : ControllerBase
    {
        private readonly IPushPlaylistsToSyncQueueService _pushPlaylistsToSyncQueueService;

        public SpotifyPlaylistsController(IPushPlaylistsToSyncQueueService pushPlaylistsToSyncQueueService)
        {
            _pushPlaylistsToSyncQueueService = pushPlaylistsToSyncQueueService;
        }

        [HttpPost("AddPlaylistsInSyncQueuue")]
        public async Task<IActionResult> AddPlaylistsInSyncQueue([FromQuery]string queueType)
        {
            try
            {
                await _pushPlaylistsToSyncQueueService.PushPlaylists(queueType);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }

    }
}
