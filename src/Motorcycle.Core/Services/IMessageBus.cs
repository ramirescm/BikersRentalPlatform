namespace Motorcycle.Core.Services;

public interface IMessageBus
{
    Task PublishAsync<T>(string topic, T message);
}