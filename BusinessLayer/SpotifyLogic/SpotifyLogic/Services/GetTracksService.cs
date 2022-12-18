using AutoMapper;
using ISpotifyLogic.DTOs.Response;
using ISpotifyLogic.Services;
using ISpotifyLogic.ViewModels.Request;
using SpotifyLib.Interfaces;

namespace SpotifyService.Services.Implementation
{
    public class GetTracksService : IGetTracksService
    {
        private readonly ISpotifyClient _spotifyClient;
        private readonly IMapper _mapper;

        public GetTracksService(ISpotifyClient spotifyClient, IMapper mapper)
        {
            _spotifyClient = spotifyClient;
            _mapper = mapper;
        }
        public async Task<TracksMetadataResponse> GetTracks(SpotifyGetTracksRequestParameters parameters)
        {
            var spotifyResult= await _spotifyClient.TracksClient.GetTracks(parameters.Offset, parameters.Limit);
            var result = _mapper.Map<TracksMetadataResponse>(spotifyResult);
            return result;
        }
    }
}
