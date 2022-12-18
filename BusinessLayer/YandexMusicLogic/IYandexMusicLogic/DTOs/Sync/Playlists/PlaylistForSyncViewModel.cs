using Yandex.Music.Api.Models.Track;

namespace IYandexMusicLogic.DTOs.Playlists
{
    public record PlaylistForSyncViewModel(string Name, List<YTrack> Tracks);
}
