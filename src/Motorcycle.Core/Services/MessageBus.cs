namespace Motorcycle.Core.Services;

using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class MessageBus : IMessageBus
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public MessageBus(string hostName, string userName, string password)
    {
        var factory = new ConnectionFactory() { HostName = hostName, UserName = userName, Password = password };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task PublishAsync<T>(string topic, T message)
    {
        //_channel.ExchangeDeclare(exchange: "motorcycle_events", type: ExchangeType.Fanout);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        _channel.BasicPublish(exchange: "", routingKey: topic, basicProperties: null, body: body);

        return Task.CompletedTask;
    }

    public void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
    }
}
