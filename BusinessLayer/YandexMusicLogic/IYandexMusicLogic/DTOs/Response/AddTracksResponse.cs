namespace IYandexMusicLogic.DTOs.Response
{
    public record AddTracksResponse(IList<AddTrackResponse> Tracks, bool IsSuccess, string ExceptionString);

    public record AddTrackResponse(string Id, string ArtistName, string TrackName, bool IsSuccessAdded);
}
