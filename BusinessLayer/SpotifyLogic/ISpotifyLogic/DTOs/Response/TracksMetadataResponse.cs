namespace ISpotifyLogic.DTOs.Response
{
    public record TracksMetadataResponse(IList<TrackMetadataResponse> Tracks, string ExceptionString);

    public record TrackMetadataResponse(string ArtistName, string TrackName, string AlbumName, IList<TrackImageInfo> TrackImages);

    public record TrackImageInfo(string Height, string Uri, string Width);
}
