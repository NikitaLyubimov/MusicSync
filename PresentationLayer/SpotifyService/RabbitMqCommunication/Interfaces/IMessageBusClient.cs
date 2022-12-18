using SpotifyService.DTOs.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpotifyService.RabbitMqCommunication.Interfaces
{
    public interface IMessageBusClient
    {
        bool PublishEntityForSync<T>(T entity, string routingKey);
    }
}
