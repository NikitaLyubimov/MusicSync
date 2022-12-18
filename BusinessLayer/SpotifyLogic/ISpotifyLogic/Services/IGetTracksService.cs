using ISpotifyLogic.DTOs.Response;
using ISpotifyLogic.ViewModels.Request;

namespace ISpotifyLogic.Services
{
    public interface IGetTracksService
    {
        Task<TracksMetadataResponse> GetTracks(SpotifyGetTracksRequestParameters parameters);
    }
}
