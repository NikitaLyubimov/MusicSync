using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;


using SpotifyLib.Interfaces;
using SpotifyLib.Clients;

using SpotifyService.ViewModels;
using SpotifyService.Services.Interfaces;
using SpotifyService.Services.Implementation;
using SpotifyService.RabbitMqCommunication.Interfaces;
using SpotifyService.RabbitMqCommunication.Implementations;
using SpotifyService.Automapper;

namespace SpotifyService
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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SpotifyService", Version = "v1" });
            });
            services.Configure<SpotifyCredsViewModel>(Configuration.GetSection("Spotify"));

            var regCreds = Configuration.GetSection("Spotify");

            services.AddAutoMapper(typeof(AppMappingProfile));

            services.AddSingleton<ISpotifyClient, SpotifyClient>(_ => new SpotifyClient(regCreds["ClientId"], regCreds["ClientSecret"]));
            services.AddTransient<ISynchroniseTracksService, SynchroniseTracksService>();
            services.AddTransient<IPushTracksToSyncQueueService, PushTracksToSyncQueueService>();
            services.AddTransient<IGetTracksService, GetTracksService>();
            services.AddTransient<IPushPlaylistsToSyncQueueService, PushPlaylistsToSyncQueueService>();

            services.AddSingleton<IMessageBusClient, MessageBusClient>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SpotifyService v1"));
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
