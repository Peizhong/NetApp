using System;
using System.Collections.Generic;
using System.Text;
using StackExchange.Redis;

namespace NetApp.Play.Misc
{
    public class Redis
    {
        ConnectionMultiplexer redis = ConnectionMultiplexer.Connect("192.168.1.108");

        public void Hello()
        {
            IDatabase db = redis.GetDatabase();
            string key = "hello";
            string value = "world";
            db.StringSet(key, value);

            var value2 = db.StringGet(key);

        }
    }
}
