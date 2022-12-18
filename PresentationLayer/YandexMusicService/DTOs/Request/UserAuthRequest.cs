using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YandexMusicService.DTOs.Request
{
    public class UserAuthRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
