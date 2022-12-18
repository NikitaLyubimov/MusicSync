namespace ISpotifyLogic.Services
{
    public interface IPushPlaylistsToSyncQueueService
    {
        Task PushPlaylists(string queuetype);
    }
}
