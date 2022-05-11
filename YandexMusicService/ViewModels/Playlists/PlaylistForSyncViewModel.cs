using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Yandex.Music.Api.Models.Track;

namespace YandexMusicService.ViewModels.Playlists
{
    public class PlaylistForSyncViewModel
    {
        public string Name { get; set; }
        public List<YTrack> Tracks { get; set; }
    }
}
