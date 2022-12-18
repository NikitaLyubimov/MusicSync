using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using AutoMapper;
using SpotifyLib.DTO.Playlists;
using SpotifyLib.Interfaces;
using SpotifyService.DTOs.Response;
using SpotifyService.RabbitMqCommunication.Interfaces;
using SpotifyService.Services.Interfaces;

using CoreLib.Playlists;
using CoreLib.TracksDTOs;

namespace SpotifyService.Services.Implementation
{
    public class PushPlaylistsToSyncQueueService : IPushPlaylistsToSyncQueueService
    {
        private ISpotifyClient _spotifyClient;
        private IMessageBusClient _messageBusClient;
        private IMapper _mapper;

        private int _totalPlaylistsCount;

        public PushPlaylistsToSyncQueueService(ISpotifyClient spotifyClient, IMessageBusClient messageBusClient, IMapper mapper)
        {
            _spotifyClient = spotifyClient;
            _messageBusClient = messageBusClient;
            _mapper = mapper;
        }
        public async Task PushPlaylists(string queuetype)
        {
            var playlists = await GetSpotifyPlaylistsWithTracks();
            var pushPlaylistsToQueueTasks = playlists.Playlists.Select(playlist => PushPlaylistsToQueue(playlist));
            await Task.WhenAll(pushPlaylistsToQueueTasks);
        }

        private async Task<bool> PushPlaylistsToQueue(PlaylistForQueue playlist)
        {
            return await Task.Run(() => _messageBusClient.PublishEntityForSync(playlist, "playlists"));
        }

        private async Task<PlaylistsForQueueDto> GetSpotifyPlaylistsWithTracks()
        {
            var batchSize = 50;

            var finalPlaylistsList = await GetSpotifyPlaylistsWithTracks(0, batchSize);

            if(_totalPlaylistsCount > batchSize)
            {
                var playlistsAmountList = Enumerable.Range(1, _totalPlaylistsCount / batchSize);
                var numberOfBatches = (int)Math.Ceiling((double)playlistsAmountList.Count() / batchSize);

                for(int i = 1; i < numberOfBatches; i++)
                {
                    var result = i < _totalPlaylistsCount / batchSize ? await GetSpotifyPlaylistsWithTracks(i * batchSize, batchSize)
                                                                      : await GetSpotifyPlaylistsWithTracks(i * batchSize, _totalPlaylistsCount - i * batchSize);
                    finalPlaylistsList.Playlists.AddRange(result.Playlists);
                }
            }

            return finalPlaylistsList;
        }

        private async Task<PlaylistsForQueueDto> GetSpotifyPlaylistsWithTracks(int offet, int limit)
        {
            var result = await GetSpotifyPlaylists(offet, limit);
            var addTracksToPlaylistsTasks = result.Playlists.Select(playlist => AddTracksToPlaylist(playlist));
            await Task.WhenAll(addTracksToPlaylistsTasks);
            return result;
        }
        

        private async Task AddTracksToPlaylist(PlaylistForQueue playlist)
        {
            var spotifyResponse = await _spotifyClient.PlaylistsClient.GetPlaylistTracks(playlist.Id);
            var trackList = _mapper.Map<List<TrackDtoResponse>>(spotifyResponse.Items);
            playlist.Tracks = trackList;
        }

        private async Task<PlaylistsForQueueDto> GetSpotifyPlaylists(int limit = 0, int offset = 0)
        {
            var spotifyResponse = await _spotifyClient.PlaylistsClient.GetCurrentUserPlaylists(limit, offset);
            if (_totalPlaylistsCount == 0)
                _totalPlaylistsCount = spotifyResponse.Total;

            var response = _mapper.Map<PlaylistsForQueueDto>(spotifyResponse);
            return response;
        }

    }
}
