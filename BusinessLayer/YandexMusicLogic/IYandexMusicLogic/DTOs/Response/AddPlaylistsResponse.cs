namespace IYandexMusicLogic.DTOs.Response
{
    public record AddPlaylistsResponse(List<AddPlaylistResponse> Playlists, bool IsSuccess, string ExceptionString);

    public record AddPlaylistResponse(string Name, bool IsSuccess);
}
