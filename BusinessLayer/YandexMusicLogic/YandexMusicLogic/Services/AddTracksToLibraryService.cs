using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Track;

using CoreLib.TracksDTOs;
using IYandexMusicLogic.Services;
using CoreLib.Utils.Interfaces;
using IYandexMusicLogic.DTOs.Response;

namespace YandexMusicLogic.Services
{
    public class AddTracksToLibraryService : LibraryCommunication, IAddTracksToLibraryService
    {
        private readonly IRetryHandler<AddTrackResponse> _retryHandler;

        public AddTracksToLibraryService(YandexMusicApi yandexMusicApi, AuthStorage authStorage, IRetryHandler<AddTrackResponse> retryHandler) : base(yandexMusicApi, authStorage)
        {
            _retryHandler = retryHandler;
        }
        public async Task<AddTracksResponse> AddTracksToLibrary(TracksForQueueDto addTracksRequest)
        {
            var tracks = await GetTracks(addTracksRequest);

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
        
    }
}
