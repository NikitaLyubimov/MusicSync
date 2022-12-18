using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using IYandexMusicLogic.DTOs.Request;

using Yandex.Music.Api;
using Yandex.Music.Api.Common;

namespace YandexMusicService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YMusicAuthController : ControllerBase
    {
        private readonly YandexMusicApi _yandexMusicApi;
        private readonly AuthStorage _authStorage;
        public YMusicAuthController(YandexMusicApi yandexMusicApi, AuthStorage authStorage)
        {
            _yandexMusicApi = yandexMusicApi;
            _authStorage = authStorage;
        }

        [HttpPost("UserAuth")]
        public async Task<IActionResult> UserAuth([FromBody]UserAuthRequest request)
        {
            try
            {
                await _yandexMusicApi.User.AuthorizeAsync(_authStorage, request.Login, request.Password);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest();
            }
        }
    }
}
