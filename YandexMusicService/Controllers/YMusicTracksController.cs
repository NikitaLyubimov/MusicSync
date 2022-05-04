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



    }
}
