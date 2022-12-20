using Yandex.Music.Api;
using Yandex.Music.Api.Common;

using CoreLib.Playlists;
using CoreLib.TracksDTOs;
using CoreLib.Utils.Interfaces;
using IYandexMusicLogic.DTOs.Response;
using IYandexMusicLogic.DTOs.Playlists;
using IYandexMusicLogic.Services;
using Yandex.Music.Api.Models.Common;
using Yandex.Music.Api.Models.Playlist;

namespace YandexMusicLogic.Services
{
    public class AddPlaylistsToLibraryService : LibraryCommunication, IAddPlaylistsToLibraryService
    {
        private readonly IRetryHandler _retryHandler;

        public AddPlaylistsToLibraryService(YandexMusicApi yandexMusicApi, AuthStorage authStorage, IRetryHandler retryHandler)
            : base(yandexMusicApi, authStorage)
        {
            _retryHandler = retryHandler;
        }
        public async Task<AddPlaylistsResponse> AddPlaylistsToLibrary(PlaylistsForQueueDto playlistsForQueue)
        {
            

            var getPlaylistsForSyncTasks = playlistsForQueue.Playlists.Select(p => GetPlaylistForSync(p));
            var playlists = await Task.WhenAll(getPlaylistsForSyncTasks);
            var createPlaylistsTasks = playlists.Select(p => AddPlaylistsToLibrary(p));
            var addPlaylistsResults = await Task.WhenAll(createPlaylistsTasks);

            var response = new AddPlaylistsResponse(addPlaylistsResults.ToList(), false, string.Empty);
            if (!addPlaylistsResults.Any(pr => pr.IsSuccess))
                return response;
            return response with { IsSuccess = true };
        }

        private async Task<PlaylistForSyncViewModel> GetPlaylistForSync(PlaylistForQueue playlist)
        {
            var tracks = await GetTracks(new TracksForQueueDto { Tracks = playlist.Tracks });
            return new PlaylistForSyncViewModel(playlist.Name, tracks.Select(t => t.Result[0]).ToList());
        }

        private async Task<AddPlaylistResponse> AddPlaylistsToLibrary(PlaylistForSyncViewModel playlist)
        {
            var response = new AddPlaylistResponse(playlist.Name, false);

            YResponse<YPlaylist> createPlaylistResponse = null;
            YResponse<YPlaylist> tracksAddedResponse = null;
            try
            {
                createPlaylistResponse = await _yandexMusicApi.Playlist.CreateAsync(_authStorage, playlist.Name);
            }
            catch (Exception ex)
            {
                createPlaylistResponse =  await _retryHandler.HandleRetry(async () => await _yandexMusicApi.Playlist.CreateAsync(_authStorage, playlist.Name));
            }
            try
            {
                tracksAddedResponse =  await _yandexMusicApi.Playlist.InsertTracksAsync(_authStorage, createPlaylistResponse.Result,
                                                                                            playlist.Tracks.ToArray());
            }
            catch(Exception ex)
            {
                tracksAddedResponse = await _retryHandler.HandleRetry(async () => await _yandexMusicApi.Playlist.InsertTracksAsync(_authStorage, createPlaylistResponse.Result,
                                                                                            playlist.Tracks.ToArray()));
            }

            if (createPlaylistResponse.Result != null && tracksAddedResponse.Result != null)
                return response with { IsSuccess = true };
            return response;
        }

    }
}
