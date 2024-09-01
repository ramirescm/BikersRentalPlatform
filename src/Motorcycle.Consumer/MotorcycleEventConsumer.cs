using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using IModel = RabbitMQ.Client.IModel;

namespace Motorcycle.Consumer;

public class MotorcycleEventConsumer : BackgroundService
{
    private readonly ILogger<MotorcycleEventConsumer> _logger;
    private IConnection _connection;
    private IModel _channel;
    private IConfiguration _configuration;
    private readonly MotorcycleEventService _eventService;
    
    public MotorcycleEventConsumer(ILogger<MotorcycleEventConsumer> logger, 
        IConfiguration configuration,
        MotorcycleEventService eventService)
    {
        _logger = logger;
        _configuration = configuration;
        _eventService = eventService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory() { HostName = _configuration["RabbitMQ:HostName"], UserName = _configuration["RabbitMQ:UserName"], Password = _configuration["RabbitMQ:Password"] };
        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
        _channel.QueueDeclare(queue: _configuration["RabbitMQ:QueueName"], durable: false, exclusive: false, autoDelete: false, arguments: null);
        
        while (!stoppingToken.IsCancellationRequested)
        {
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);
                    
                    var motorcycleEvent = JsonSerializer.Deserialize<MotorcycleRegisteredEvent>(message);
                    if (motorcycleEvent.Year == 2024)
                    {
                        _eventService.SaveEventToDatabase(motorcycleEvent);
                    }
                    
                    _channel.BasicAck(ea.DeliveryTag, false);
                }
                catch (Exception ex)
                {
                    _logger.LogError("Error process notification", ex);
                    _channel.BasicNack(ea.DeliveryTag, false, true);
                }
            };

            _channel.BasicConsume(queue: _configuration["RabbitMQ:QueueName"],
                autoAck: false,
                consumer: consumer);

        }

        await Task.CompletedTask;
    }
    
    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }
}

