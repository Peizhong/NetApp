using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.CeleryTask
{
    internal class MongoLogger
    {
        internal class LogMessage
        {
            [BsonId]
            public ObjectId Id { get; set; }

            // ObjectId 保存了创建的时间戳
            [BsonElement("eventTime")]
            public DateTime EventTime { get; set; }

            [BsonElement("level")]
            public LogLevel Level { get; set; }

            [BsonElement("logger")]
            public string Logger { get; set; }

            [BsonElement("message")]
            public string Message { get; set; }

            [BsonElement("exception")]
            public Exception Exception { get; set; }
        }


        private static MongoLogger _logger = null;

        private MongoLogger() { }

        private static readonly object _locker = new object();

        public static MongoLogger Instance
        {
            get
            {
                if (_logger == null)
                {
                    lock (_locker)
                    {
                        if (_logger == null)
                        {
                            _logger = new MongoLogger();
                        }
                    }
                }
                return _logger;
            }
        }

        MongoClient _client;

        public async Task<bool>  TestConnectionAsync()
        {
            _client = new MongoClient("mongodb://192.168.3.19:27017");
            var database = _client.GetDatabase("CeleryLogDB");
            var collection = database.GetCollection<LogMessage>("CeleryLog");
            var all = await collection.Find(_ => true).ToListAsync();
            var main =  await collection.Find(z => z.Logger == "Main").ToListAsync();
            if (all.Count < 10)
            {
                await collection.InsertOneAsync(new LogMessage
                {
                    EventTime = DateTime.Now,
                    Level = LogLevel.Information,
                    Logger = "Main",
                    Message = "Test",
                    Exception = new Exception("Error")
                });
                await collection.Find(z => z.Logger == "Main").ToListAsync();
            }
            return true;
        }

        public void LogInformation(string msg)
        {
        }
    }
}
