using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

using YandexMusicService.DTOs.Request;
using YandexMusicService.Services.Interfaces;
using System;

namespace YandexMusicService.Services.Implementation
{
    public class MessageBusSubscriber : BackgroundService
    {
        private IConfiguration _configuration;
        private IConnection _connection;
        private IModel _channel;
        private string _queueName;

        private IAddTracksToLibraryService _addTracksToLibraryService;

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
                Port = int.Parse(_configuration["RabbitMqPort"])
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.ExchangeDeclare(
                exchange: "musicsync",
                type: ExchangeType.Fanout);
            _queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(
                queue: _queueName,
                exchange: "musicsync",
                routingKey: "");

        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            stoppingToken.ThrowIfCancellationRequested();

            var consumer = new EventingBasicConsumer(_channel);

            consumer.Received += (bc, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var addTracksRequest = JsonConvert.DeserializeObject<AddTracksRequest>(message);
                Console.WriteLine("YEsss");
                var response = _addTracksToLibraryService.AddTracksToLibrary(addTracksRequest).GetAwaiter().GetResult();
                _channel.BasicAck(ea.DeliveryTag, false);
                //await Task.Yield();
            };
            _channel.BasicConsume(
                queue: _queueName,
                autoAck: false,
                consumer: consumer);

            return Task.CompletedTask;
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
