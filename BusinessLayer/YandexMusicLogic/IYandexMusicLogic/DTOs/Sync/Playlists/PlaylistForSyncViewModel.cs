using Yandex.Music.Api.Models.Track;

namespace IYandexMusicLogic.DTOs.Playlists
{
    public class PlaylistForSyncViewModel
    {
        public string Name { get; set; }
        public List<YTrack> Tracks { get; set; }
    }
}
