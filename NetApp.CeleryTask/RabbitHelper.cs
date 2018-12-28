using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.CeleryTask
{
    public class RabbitHelper
    {
        public static readonly RabbitHelper Instance = new RabbitHelper();

        private string _host;
        private ConnectionFactory _factory;

        public void Init(string rabbitHost)
        {
            _host = rabbitHost;
            _factory = new ConnectionFactory() { HostName = _host };
        }

        public void ExcuteOnce(Action<IModel> action)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    action?.Invoke(channel);
                }
            }
        }

        public void RegisterQueue(string queue, Action<byte[]> action)
        {
            using (var connection = _factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (model, ea) =>
                    {
                        var body = ea.Body;
                        action?.Invoke(body);
                    };
                    channel.BasicConsume(queue: queue,
                                         autoAck: true,
                                         consumer: consumer);
                }
            }
        }
    }
}
