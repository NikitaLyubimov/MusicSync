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
        private readonly IRetryHandler _retryHandler;

        public AddTracksToLibraryService(YandexMusicApi yandexMusicApi, AuthStorage authStorage, IRetryHandler retryHandler) : base(yandexMusicApi, authStorage)
        {
            _retryHandler = retryHandler;
        }
        public async Task<AddTracksResponse> AddTracksToLibrary(TracksForQueueDto addTracksRequest)
        {
            
            var tracks = await GetTracks(addTracksRequest);

            var addTrackToLibraryTasks = tracks.Select(track => AddTrackToLibrary(track.Result[0]));
            var addTrackToLibraryResult = await Task.WhenAll(addTrackToLibraryTasks);

            var response = new AddTracksResponse(addTrackToLibraryResult, false, string.Empty);
            if (!addTrackToLibraryResult.Any(res => res.IsSuccessAdded))
                return response;

            return response with { IsSuccess = true };
        }

        private async Task<AddTrackResponse> AddTrackToLibrary(YTrack track)
        {
            try
            {
                var response = await AddTrackToLibraryRequest(track);
                return response;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                var retryResponse = await _retryHandler.HandleRetry(async () => await AddTrackToLibraryRequest(track));
                return retryResponse;
            }
        }

        private async Task<AddTrackResponse> AddTrackToLibraryRequest(YTrack track)
        {
            var response = new AddTrackResponse(track.Id, track.Artists[0].Name, track.Title, false);

            var result = await _yandexMusicApi.Library.AddTrackLikeAsync(_authStorage, track);

            if (result.Result == null)
                return response;

            return response with { IsSuccessAdded = true };
        }
        
    }
}
