using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yandex.Music.Api.Common;
using Yandex.Music.Api;
using IYandexMusicLogic.Services;
using YandexMusicLogic.Services;
using YandexMusicLogic.BackgroundServices;
using CoreLib.Utils.Implementations;
using CoreLib.Utils.Interfaces;

namespace YandexMusicLogic
{
    public static class YandexMusicLogicCollectionExtension
    {
        public static IServiceCollection AddYandexMusicLogic(this IServiceCollection services)
        {
            services.AddHostedService<MessageBusSubscriber>();
            services.AddSingleton<YandexMusicApi>();
            services.AddSingleton<AuthStorage>();

            services.AddTransient<IAddTracksToLibraryService, AddTracksToLibraryService>();
            services.AddTransient<IAddPlaylistsToLibraryService, AddPlaylistsToLibraryService>();

            services.AddTransient<IRetryHandler, RetryHandler>();

            services.AddSingleton<YandexMusicApi>();
            services.AddSingleton<AuthStorage>();

            return services;
        }
    }
}
