using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.RabbitMqCommunication.Interfaces;
using SpotifyService.Services.Interfaces;

using AutoMapper;

using CoreLib.TracksDTOs;

namespace SpotifyService.Services.Implementation
{
    public class PushTracksToSyncQueueService : IPushTracksToSyncQueueService
    {
        private ISpotifyClient _spotifyClient;
        private IMessageBusClient _messageBusClient;
        private IMapper _mapper;
        private int _spotifyUserLibTotalTracks;

        public PushTracksToSyncQueueService(ISpotifyClient spotifyClient, IMessageBusClient messageBusClient, IMapper mapper)
        {
            _spotifyClient = spotifyClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }
        public async Task PushTracks(string queueType)
        {
            var batchSize = 50;

            var firstBunchOfTracks = await GetSpotifyTracks(0, batchSize);
            _messageBusClient.PublishEntityForSync(firstBunchOfTracks, "tracks");
            var getTracksAmountList = Enumerable.Range(1, _spotifyUserLibTotalTracks / batchSize);
            List<TracksForQueueDto> getTracksResults;

            
            if (_spotifyUserLibTotalTracks > batchSize)
            {
                getTracksResults = new();
                int numberOfBatches = (int)Math.Ceiling((double)getTracksAmountList.Count() / batchSize);
                for(int i = 0; i < numberOfBatches; i++)
                {
                    var currentIds = getTracksAmountList.Skip(i * batchSize).Take(batchSize);
                    var tasks = currentIds.Select(id => id < _spotifyUserLibTotalTracks / batchSize ? GetSpotifyTracks(id * batchSize, batchSize)
                                                                : GetSpotifyTracks(id * batchSize, _spotifyUserLibTotalTracks - id * batchSize));

                    getTracksResults.AddRange(await Task.WhenAll(tasks));
                }
            }
            else
            {
                var tasks = getTracksAmountList.Select(id => id <= _spotifyUserLibTotalTracks / batchSize ? GetSpotifyTracks(id * batchSize, batchSize)
                                                                : GetSpotifyTracks(id * batchSize, _spotifyUserLibTotalTracks - id * batchSize));
                getTracksResults = (await Task.WhenAll(tasks)).ToList();
                
            }
            var pushTracksTasks = getTracksResults.Select(result => PushTracksToQueue(result));
            var results = await Task.WhenAll(pushTracksTasks);
        }

        private async Task<bool> PushTracksToQueue(TracksForQueueDto tracks)
        {
            return await Task.Run(() => _messageBusClient.PublishEntityForSync(tracks, "tracks"));
            
        }

        private async Task<TracksForQueueDto> GetSpotifyTracks(int offset, int limit)
        {
            var spotifyResponse = await _spotifyClient.TracksClient.GetTracks(offset, limit);
            if (_spotifyUserLibTotalTracks == 0)
                _spotifyUserLibTotalTracks = spotifyResponse.Total;
            var response = _mapper.Map<TracksForQueueDto>(spotifyResponse); 
            return response;
        }
    }
}
