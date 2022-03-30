using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using YandexMusicService.DTOs.Request;
using YandexMusicService.Services.Interfaces;

using Microsoft.Extensions.DependencyInjection;
using YandexMusicService.DTOs.Response;

namespace YandexMusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YMusicTracksController : ControllerBase
    {
        private readonly YandexMusicApi _yandexMusicApi;
        private readonly AuthStorage _authStorage;

        public YMusicTracksController(YandexMusicApi yandexMusicApi, AuthStorage authStorage)
        {
            _yandexMusicApi = yandexMusicApi;
            _authStorage = authStorage;
        }

        [HttpPost("AddTracksToLibrary")]
        public async Task<IActionResult> AddTracksToLibrary([FromBody]AddTracksRequest addTracksRequest)
        {
            try
            {
                var addTracksService = HttpContext.RequestServices.GetService<IAddTracksToLibraryService>();
                var result = await addTracksService.AddTracksToLibrary(addTracksRequest);

                return result.IsSuccess ? Ok(result) : BadRequest(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new AddTracksResponse { ExceptionString = "Unknown error while adding tracks to library", IsSuccess = false });
            }
        }


    }
}
