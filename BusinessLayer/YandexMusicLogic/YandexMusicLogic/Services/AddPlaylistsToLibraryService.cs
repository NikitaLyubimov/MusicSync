using Yandex.Music.Api;
using Yandex.Music.Api.Common;

using CoreLib.Playlists;
using CoreLib.TracksDTOs;
using CoreLib.Utils.Interfaces;
using IYandexMusicLogic.DTOs.Response;
using IYandexMusicLogic.DTOs.Playlists;
using IYandexMusicLogic.Services;

namespace YandexMusicLogic.Services
{
    public class AddPlaylistsToLibraryService : LibraryCommunication, IAddPlaylistsToLibraryService
    {
        private readonly IRetryHandler<AddTrackResponse> _retryHandler;

        public AddPlaylistsToLibraryService(YandexMusicApi yandexMusicApi, AuthStorage authStorage, IRetryHandler<AddTrackResponse> retryHandler)
            : base(yandexMusicApi, authStorage)
        {
            _retryHandler = retryHandler;
        }
        public async Task<AddPlaylistsResponse> AddPlaylistsToLibrary(PlaylistsForQueueDto playlistsForQueue)
        {
            var getPlaylistsForSyncTasks = playlistsForQueue.Playlists.Select(p => GetPlaylistForSync(p));
            var playlists = await Task.WhenAll(getPlaylistsForSyncTasks);
            var createPlaylistsTasks = playlists.Select(p => AddPlaylistsToLibrary(p));
            await Task.WhenAll(createPlaylistsTasks);
            return new AddPlaylistsResponse();
        }

        private async Task<PlaylistForSyncViewModel> GetPlaylistForSync(PlaylistForQueue playlist)
        {
            var tracks = await GetTracks(new TracksForQueueDto { Tracks = playlist.Tracks });
            return new PlaylistForSyncViewModel { Name = playlist.Name, Tracks = tracks.Select(t => t.Result[0]).ToList() };
        }

        private async Task AddPlaylistsToLibrary(PlaylistForSyncViewModel playlist)
        {
            var createPlaylistResponse = await _yandexMusicApi.Playlist.CreateAsync(_authStorage, playlist.Name);
            var insertTracksResponse = await _yandexMusicApi.Playlist.InsertTracksAsync(_authStorage, createPlaylistResponse.Result, 
                                                                                            playlist.Tracks.ToArray());

        }

    }
}
