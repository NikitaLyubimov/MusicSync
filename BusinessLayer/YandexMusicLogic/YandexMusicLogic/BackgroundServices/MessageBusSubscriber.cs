using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using CoreLib.TracksDTOs;
using Microsoft.Extensions.DependencyInjection;
using IYandexMusicLogic.Services;
using System.Text.Json;

namespace YandexMusicLogic.BackgroundServices
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        private IAddTracksToLibraryService _addTracksToLibraryService;
        private IServiceProvider _serviceProvider;

        public MessageBusSubscriber(IConfiguration configuration, IAddTracksToLibraryService addTracksToLibraryService)
        {
            _configuration = configuration;
            _addTracksToLibraryService = addTracksToLibraryService;

            InitRabbitMqConnection();
        }

        private void InitRabbitMqConnection()
        {
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMqHost"],
                Port = int.Parse(_configuration["RabbitMqPort"]),
                DispatchConsumersAsync = true
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: "musicsync",
                type: ExchangeType.Direct);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                queue: _queueName,
                exchange: "musicsync",
                routingKey: "playlists");

            _channel.QueueBind(
                queue: _queueName,
                exchange: "musicsync",
                routingKey: "tracks"
                );

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.Received +=  async (bc, ea) =>
            {
                var body = ea.Body.ToArray();
                var routingKey = ea.RoutingKey;
                var message = Encoding.UTF8.GetString(body);
                await ExecuteSynchronization(routingKey, message);
                _channel.BasicAck(ea.DeliveryTag, false);
            };
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
        }

        private async Task ExecuteSynchronization(string routingKey, string message)
        {
            switch (routingKey)
            {
                case "tracks":
                    var addTracksService = _serviceProvider.GetRequiredService<IAddTracksToLibraryService>();
                    var addTracksRequest = JsonSerializer.Deserialize<TracksForQueueDto>(message);
                    await addTracksService.AddTracksToLibrary(addTracksRequest);
                    break;
                
            }
        }


        public override void Dispose()
        {
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
            base.Dispose();
        }
    }
}
