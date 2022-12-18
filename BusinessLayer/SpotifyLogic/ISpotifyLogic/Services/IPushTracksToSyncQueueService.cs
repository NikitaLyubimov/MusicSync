namespace ISpotifyLogic.Services
{
    public interface IPushTracksToSyncQueueService
    {
        Task PushTracks(string queueType);
    }
}
