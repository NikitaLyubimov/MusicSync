using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YandexMusicService.DTOs.Response
{
    public class UserAuthResponse
    {
        public string Email { get; set; }
        public bool IsSuccess { get; set; }
        public string ExceptionString { get; set; }
    }
}
