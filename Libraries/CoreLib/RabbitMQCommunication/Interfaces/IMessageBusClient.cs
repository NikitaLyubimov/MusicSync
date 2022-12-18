namespace CoreLib.RabbitMQCommunication.Interfaces
{
    public interface IMessageBusClient
    {
        bool PublishEntityForSync<T>(T entity, string routingKey);
    }
}
