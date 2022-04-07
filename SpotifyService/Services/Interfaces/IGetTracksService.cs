using System.Threading.Tasks;

using SpotifyService.DTOs.Response;
using SpotifyService.ViewModels.Request;

namespace SpotifyService.Services.Interfaces
{
    public interface IGetTracksService
    {
        Task<TracksMetadataResponse> GetTracks(SpotifyGetTracksRequestParameters parameters);
    }
}
