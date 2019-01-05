using NetApp.CeleryTask.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.CeleryTask
{
    public class RabbitHelper
    {
        public static readonly RabbitHelper Instance = new RabbitHelper();

        private ConnectionFactory _factory;
        private IConnection _connection;

        private Dictionary<string, EventingBasicConsumer> _consumerDict;

        public void Init(string rabbitHost)
        {
            _factory = new ConnectionFactory() { HostName = rabbitHost };
            _consumerDict = new Dictionary<string, EventingBasicConsumer>();
        }

        public IConnection OpenedConnection
        {
            get
            {
                if (_connection?.IsOpen != true)
                {
                    _connection = _factory.CreateConnection();
                }
                return _connection;
            }
        }

        /// <summary>
        /// 声明支持的任务
        /// </summary>
        /// <param name="queueName"></param>
        /// <returns></returns>
        public QueueDeclareOk DeclareQueue(string queueName)
        {
            using (var channel = OpenedConnection.CreateModel())
            {
                var res = channel.QueueDeclare(queue: queueName,
                                      durable: false,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                return res;
            }
        }

        public void Pubilsh(PeriodicTask task)
        {
            using (var channel = OpenedConnection.CreateModel())
            {
                var body = Encoding.UTF8.GetBytes( "hello");

                channel.BasicPublish(exchange: "",
                                      routingKey: task.TaskName,
                                      basicProperties: null,
                                      body: body);
            }
        }

        /// <summary>
        /// 订阅任务
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string RegisterQueue(string queueName, Action<byte[]> action)
        {
            var key = _consumerDict.Keys.FirstOrDefault(k => k.StartsWith(queueName));
            if (!string.IsNullOrWhiteSpace(key))
            {
                return key;
            }
            var channel = OpenedConnection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                action?.Invoke(body);
            };
            key = $"{queueName}_{Guid.NewGuid().ToString("N")}";
            var res = channel.BasicConsume(queue: queueName,
                                 autoAck: true,
                                 consumer: consumer,
                                 consumerTag: key);
            _consumerDict.Add(key, consumer);
            return res;
        }
    }
}
