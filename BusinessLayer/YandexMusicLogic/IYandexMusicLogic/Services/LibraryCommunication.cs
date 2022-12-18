using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Track;

using CoreLib.TracksDTOs;

namespace IYandexMusicLogic.Services
{
    public abstract class LibraryCommunication
    {
        protected readonly YandexMusicApi _yandexMusicApi;
        protected readonly AuthStorage _authStorage;

        public LibraryCommunication(YandexMusicApi yandexMusicApi, AuthStorage authStorage)
        {
            _yandexMusicApi = yandexMusicApi;
            _authStorage = authStorage;
        }

        protected async Task<List<YResponse<List<YTrack>>>> GetTracks(TracksForQueueDto addTracksRequest)
        {
            var trackIds = await GetTracksIds(addTracksRequest);

            return await GetTracks(trackIds);
        }

        internal async Task<List<YResponse<List<YTrack>>>> GetTracks(List<string> ids)
        {
            var getTrackByIdTasks = ids.Select(id => _yandexMusicApi.Track.GetAsync(_authStorage, id));
            var getTrackByIdResults = await Task.WhenAll(getTrackByIdTasks);
            return getTrackByIdResults.ToList();
        }

        internal async Task<List<string>> GetTracksIds(TracksForQueueDto addTracksRequest)
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
