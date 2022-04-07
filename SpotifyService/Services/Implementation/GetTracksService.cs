using AutoMapper;
using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.Services.Interfaces;
using SpotifyService.ViewModels.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
