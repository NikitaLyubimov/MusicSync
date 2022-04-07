using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.RabbitMqCommunication.Interfaces;
using SpotifyService.Services.Interfaces;

using AutoMapper;

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
            var firstBunchOfTracks = await GetSpotifyTracks(0, 50);
            _messageBusClient.PublishTracksForSync(firstBunchOfTracks);
            var getTracksAmountList = Enumerable.Range(1, _spotifyUserLibTotalTracks / 50);
            List<TracksForQueueResponse> getTracksResults;
            if(_spotifyUserLibTotalTracks > 50)
            {
                getTracksResults = new();
                var batchSize = 50;
                int numberOfBatches = (int)Math.Ceiling((double)getTracksAmountList.Count() / batchSize);
                for(int i = 0; i < numberOfBatches; i++)
                {
                    var currentIds = getTracksAmountList.Skip(i * batchSize).Take(batchSize);
                    var tasks = currentIds.Select(id => id < _spotifyUserLibTotalTracks / 50 ? GetSpotifyTracks(id * 50, 50)
                                                                : GetSpotifyTracks(id * 50, _spotifyUserLibTotalTracks - id * 50));

                    getTracksResults.AddRange(await Task.WhenAll(tasks));
                }
            }
            else
            {
                var tasks = getTracksAmountList.Select(id => id <= _spotifyUserLibTotalTracks / 50 ? GetSpotifyTracks(id * 50, 50)
                                                                : GetSpotifyTracks(id * 50, _spotifyUserLibTotalTracks - id * 50));
                getTracksResults = (await Task.WhenAll(tasks)).ToList();
                
            }
            var pushTracksTasks = getTracksResults.Select(result => PushTracksToQueue(result));
            var results = await Task.WhenAll(pushTracksTasks);
        }

        private async Task<bool> PushTracksToQueue(TracksForQueueResponse tracks)
        {
            return await Task.Run(() => _messageBusClient.PublishTracksForSync(tracks));
            
        }

        private async Task<TracksForQueueResponse> GetSpotifyTracks(int offset, int limit)
        {
            var spotifyResponse = await _spotifyClient.TracksClient.GetTracks(offset, limit);
            if (_spotifyUserLibTotalTracks == 0)
                _spotifyUserLibTotalTracks = spotifyResponse.Total;
            var response = _mapper.Map<TracksForQueueResponse>(spotifyResponse); 
            return response;
        }
    }
}
