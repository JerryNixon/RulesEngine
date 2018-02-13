using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Diagnostics;
using System.Text;

namespace RuleEngine.Queue
{
    // https://www.rabbitmq.com/tutorials/tutorial-two-dotnet.html

    public class MessageService
    {
        private IConfiguration _configuration;
        private ConnectionFactory _factory;

        public MessageService()
            : this(new Configuration())
        {
            // empty
        }

        public MessageService(IConfiguration configuration)
        {
            _configuration = configuration;
            _factory = new ConnectionFactory()
            {
                HostName = _configuration.MessageQueueHost
            };
            if (!TryCreateQueue(out var result))
            {
                throw new Exception("TryCreateQueue failed.");
            }
        }

        public bool TryCreateQueue(out QueueDeclareOk result)
        {
            try
            {
                var value = default(QueueDeclareOk);
                RunInChannel(channel =>
                {
                    value = channel.QueueDeclare(
                        queue: _configuration.MessageQueueName,
                        durable: true,
                        exclusive: false,
                        autoDelete: false,
                        arguments: null);
                });
                result = value;
                return true;
            }
            catch (Exception)
            {
                Debugger.Break();
                result = default(QueueDeclareOk);
                return false;
            }
        }

        public bool TrySend(string message)
        {
            try
            {
                RunInChannel(channel =>
                {
                    var body = Encoding.UTF8.GetBytes(message);
                    var properties = channel.CreateBasicProperties();
                    properties.Persistent = true;
                    channel.BasicPublish(
                        exchange: "MainExchange",
                        routingKey: "MainRoute",
                        basicProperties: properties,
                        body: body);
                });
                return true;
            }
            catch (Exception)
            {
                Debugger.Break();
                return false;
            }
        }

        public bool TrySetupListener(Func<string, bool> action)
        {
            try
            {
                RunInChannel(channel =>
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        if (action(message))
                        {
                            channel.BasicAck(deliveryTag: e.DeliveryTag, multiple: false);
                        }
                    };
                    channel.BasicConsume(
                        queue: _configuration.MessageQueueName,
                        autoAck: true,
                        consumer: consumer);
                });
                return true;
            }
            catch (Exception)
            {
                Debugger.Break();
                return false;
            }
        }

        private void RunInChannel(Action<IModel> action)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    channel.BasicQos(
                        prefetchSize: 0,
                        prefetchCount: 1,
                        global: false);
                    action(channel);
                }
            }
        }
    }
}
