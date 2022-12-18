using ISpotifyLogic.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SpotifyLib.Clients;
using SpotifyLib.Interfaces;
using SpotifyService.Automapper;
using SpotifyService.Services.Implementation;

namespace SpotifyLogic
{
    public static class SpotifyLogicServiceCollectionExtension
    {
        public static IServiceCollection AddSpotifyLogic(this IServiceCollection services, IConfiguration configuration)
        {
            var regCreds = configuration.GetSection("Spotify");

            services.AddSingleton<ISpotifyClient, SpotifyClient>(_ => new SpotifyClient(regCreds["ClientId"], regCreds["ClientSecret"]));
            services.AddTransient<IPushTracksToSyncQueueService, PushTracksToSyncQueueService>();
            services.AddTransient<IGetTracksService, GetTracksService>();
            services.AddTransient<IPushPlaylistsToSyncQueueService, PushPlaylistsToSyncQueueService>();

            services.AddAutoMapper(typeof(AppMappingProfile));

            return services;

        }
    }
}
