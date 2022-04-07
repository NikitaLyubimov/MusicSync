﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using SpotifyService.DTOs.Response;
using SpotifyService.RabbitMqCommunication.Interfaces;

namespace SpotifyService.RabbitMqCommunication.Implementations
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _chanel;
        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory() { HostName = _configuration["RabbitMqHost"], Port = int.Parse(_configuration["RabbitMqPort"]) };

            try
            {
                _connection = factory.CreateConnection();
                _chanel = _connection.CreateModel();

                _chanel.ExchangeDeclare(exchange: "musicsync", type: ExchangeType.Fanout);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        public bool PublishTracksForSync(TracksForQueueResponse tracks)
        {
            var message = JsonSerializer.Serialize(tracks);

            if (_connection.IsOpen)
            {
                SendMessage(message);
                return true;
            }
            else
            {
                Console.WriteLine("Channel closed");
                return false;
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _chanel.BasicPublish(exchange: "musicsync",
                                routingKey: "",
                                basicProperties: null,
                                body: body);
        }
    }
}
