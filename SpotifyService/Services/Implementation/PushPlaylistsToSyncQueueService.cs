using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;

using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.RabbitMqCommunication.Interfaces;
using SpotifyService.Services.Interfaces;

namespace SpotifyService.Services.Implementation
{
    public class PushPlaylistsToSyncQueueService : IPushPlaylistsToSyncQueueService
    {
        private ISpotifyClient _spotifyClient;
        private IMessageBusClient _messageBusClient;
        private IMapper _mapper;

        private ConcurrentDictionary<string, List<TrackDtoResponse>> _tracksDictionary;

        public PushPlaylistsToSyncQueueService(ISpotifyClient spotifyClient, IMessageBusClient messageBusClient, IMapper mapper)
        {
            _spotifyClient = spotifyClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
            _tracksDictionary = new();
        }
        public async Task PushPlaylists(string queuetype)
        {
            var playlists = await GetSpotifyPlaylistsWithTracks();

        }

        private async Task<PlaylistsForQueueResponse> GetSpotifyPlaylistsWithTracks()
        {
            var result = await _spotifyClient.PlaylistsClient.GetCurrentUserPlaylists();
            List<List<TrackDtoResponse>> getTracksForPlaylistsResult; 
            if(result.Items.Count() > 50)
            {
                getTracksForPlaylistsResult = new();
                var playlistsAmountList = Enumerable.Range(1, result.Items.Count());
                var batchSize = 50;
                var numberOfBatches = (int)Math.Ceiling((double)result.Items.Count() / batchSize);

                for(int i = 0; i < numberOfBatches; i++)
                {
                    var currentResult = result.Items.Skip(i * batchSize).Take(batchSize);
                    var tasks = currentResult.Select(i => AddTracksToDictionaryForPlaylist(i.Id));
                    await Task.WhenAll(tasks);
                }
            }
            else
            {
                var tasks = result.Items.Select(i => AddTracksToDictionaryForPlaylist(i.Id));
                await Task.WhenAll(tasks);
            }


            var playlists = _mapper.Map<PlaylistsForQueueResponse>(result);
            foreach (var playlist in playlists.Playlists)
                playlist.Tracks = _tracksDictionary[playlist.SpotifyId];
            return playlists;
        }

        private async Task AddTracksToDictionaryForPlaylist(string playlistId)
        {
            var spotifyResponse = await _spotifyClient.PlaylistsClient.GetPlaylistTracks(playlistId);
            var trackList = _mapper.Map<List<TrackDtoResponse>>(spotifyResponse.Items);
            _tracksDictionary.TryAdd(playlistId, trackList);
        }

    }
}
