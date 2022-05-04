using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Track;
using YandexMusicService.DTOs.Request;
using YandexMusicService.DTOs.Response;
using YandexMusicService.Services.Interfaces;
using YandexMusicService.Utils.Interfaces;

using CoreLib.TracksDTOs;

namespace YandexMusicService.Services.Implementation
{
    public class AddTracksToLibraryService : IAddTracksToLibraryService
    {
        private readonly YandexMusicApi _yandexMusicApi;
        private readonly AuthStorage _authStorage;
        private readonly IRetryHandler _retryHandler;

        public AddTracksToLibraryService(YandexMusicApi yandexMusicApi, AuthStorage authStorage, IRetryHandler retryHandler)
        {
            _yandexMusicApi = yandexMusicApi;
            _authStorage = authStorage;
            _retryHandler = retryHandler;
        }
        public async Task<AddTracksResponse> AddTracksToLibrary(TracksForQueueDto addTracksRequest)
        {
            var tracksIds = await GetTracksIds(addTracksRequest);
            var tracks = await GetTracks(tracksIds);

            var addTrackToLibraryTasks = tracks.Select(track => AddTrackToLibrary(track.Result[0]));
            var addTrackToLibraryResult = await Task.WhenAll(addTrackToLibraryTasks);
            if (!addTrackToLibraryResult.Any(res => res.IsSuccessAdded))
                return new AddTracksResponse
                {
                    IsSuccess = false,
                    Tracks = addTrackToLibraryResult
                };

            return new AddTracksResponse
            {
                IsSuccess = true,
                Tracks = addTrackToLibraryResult
            };


        }

        private async Task<AddTrackResponse> AddTrackToLibrary(YTrack track)
        {
            try
            {
                var result = await _yandexMusicApi.Library.AddTrackLikeAsync(_authStorage, track);
                if (result.Result == null)
                    return new AddTrackResponse
                    {
                        ArtistName = track.Artists[0].Name,
                        TrackName = track.Title,
                        Id = track.Id,
                        IsSuccessAdded = false
                    };
                return new AddTrackResponse
                {
                    ArtistName = track.Artists[0].Name,
                    TrackName = track.Title,
                    Id = track.Id,
                    IsSuccessAdded = true
                };
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                var response = await _retryHandler.HandleRetry(async () =>
                {
                    var result = await _yandexMusicApi.Library.AddTrackLikeAsync(_authStorage, track);
                    return new AddTrackResponse
                    {
                        ArtistName = track.Artists[0].Name,
                        TrackName = track.Title,
                        Id = track.Id,
                        IsSuccessAdded = false
                    };

                });
                return response;
            }
        }
        private async Task<List<YResponse<List<YTrack>>>> GetTracks(List<string> ids)
        {
            var getTrackByIdTasks = ids.Select(id => _yandexMusicApi.Track.GetAsync(_authStorage, id));
            var getTrackByIdResults = await Task.WhenAll(getTrackByIdTasks);
            return getTrackByIdResults.ToList();
        }

        private async Task<List<string>> GetTracksIds(TracksForQueueDto addTracksRequest)
        {
            var idsList = new List<string>();

            var trackSearchTasks = addTracksRequest.Tracks.Select(t => GetTrackId(t.TrackName, t.ArtistName));
            var searchTrackResults = await Task.WhenAll(trackSearchTasks);
            return searchTrackResults.Where(id => !string.IsNullOrEmpty(id)).ToList();

        }

        private async Task<string> GetTrackId(string trackName, string artistName)
        {
            try
            {
                var result = await _yandexMusicApi.Search.TrackAsync(_authStorage, trackName);
                if (result == null || result.Result.Tracks == null)
                    return string.Empty;
                var findedTrack = result.Result.Tracks.Results.FirstOrDefault(t => t.Artists.Any(a => a.Name.Equals(artistName)));
                return findedTrack != null ? findedTrack.Id : string.Empty;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
