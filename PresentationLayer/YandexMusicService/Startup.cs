using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


using Yandex.Music.Api;
using Yandex.Music.Api.Common;
using YandexMusicService.Services.Implementation;
using YandexMusicService.Services.Interfaces;
using YandexMusicService.Utils.Implementations;
using YandexMusicService.Utils.Interfaces;

namespace YandexMusicService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            services.AddHostedService<MessageBusSubscriber>();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "YandexMusicService", Version = "v1" });
            });

            services.AddSingleton<YandexMusicApi>();
            services.AddSingleton<AuthStorage>();

            services.AddTransient<IAddTracksToLibraryService, AddTracksToLibraryService>();
            services.AddTransient<IAddPlaylistsToLibraryService, AddPlaylistsToLibraryService>();
            services.AddTransient<IRetryHandler, RetryHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "YandexMusicService v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}