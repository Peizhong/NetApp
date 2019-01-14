using NetApp.CeleryTask.Models;
using ProtoBuf;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.IO;
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
                                      durable: true, // don't lose the queue where server stops
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
                return res;
            }
        }

        private byte[] getPeriodTaskBytes(PeriodicTask task)
        {
            var remoteTask = new RemoteCTask
            {
                TaskName = task.TaskName,
                ParamsJSON = task.Params
            };
            using (var stream = new MemoryStream())
            {
                Serializer.Serialize(stream, remoteTask);
                var data = stream.GetBuffer();
                return data;
            }
        }

        public void Pubilsh(PeriodicTask task, string queueName)
        {
            using (var channel = OpenedConnection.CreateModel())
            {
                var body = getPeriodTaskBytes(task);
                using (var rStream = new MemoryStream(body))
                {
                    var tz = Serializer.Deserialize<RemoteCTask>(rStream);
                }

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(exchange: "",
                                      routingKey: queueName,
                                      basicProperties: properties,
                                      body: body);
            }
        }

        /// <summary>
        /// 订阅任务
        /// </summary>
        /// <param name="queueName"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public string RegisterQueue(string queueName, Func<byte[],ExcuteResult> action)
        {
            var key = _consumerDict.Keys.FirstOrDefault(k => k.StartsWith(queueName));
            if (!string.IsNullOrWhiteSpace(key))
            {
                return key;
            }
            var channel = OpenedConnection.CreateModel();
            // consumer might start before publisher 
            // is idempotent - it will only be created if it doesn't exist already
            channel.QueueDeclare(queue: queueName,
                     durable: true,
                     exclusive: false,
                     autoDelete: false,
                     arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            //server publish messages asynchronously, round-robin
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var result = action.Invoke(body);
                if (result.Code == "0000")
                {
                    channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
                }
                else
                {
                    channel.BasicReject(deliveryTag: ea.DeliveryTag, requeue: true);
                }
            };
            key = $"{queueName}_{Guid.NewGuid().ToString("N")}";
            var res = channel.BasicConsume(queue: queueName,
                                 autoAck: false,
                                 consumer: consumer,
                                 consumerTag: key);
            _consumerDict.Add(key, consumer);
            return res;
        }
    }
}
