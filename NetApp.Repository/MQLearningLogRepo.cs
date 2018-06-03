using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetApp.Entities.LearningLog;
using NetApp.Repository.Interfaces;
using MySql.Data.MySqlClient;
using Dapper;

namespace NetApp.Repository
{
    public class MQLearningLogRepo : ILearningLogRepo
    {
        static string connStr = "server=193.112.41.28;user=root;database=MYDEV;port=3306;password=mypass;SslMode=none";

        public int DeleteEntry(int entryId)
        {
            throw new NotImplementedException();
        }

        public int DeleteTopic(int topicId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Entry> GetEntries(int topicId)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var res = conn.Query<Entry>("SELECT id,title,text,link,date_add as updatetime,topic_id as topicid FROM learning_logs_entry where topic_id=@topicid;", new { topicid = topicId });
                int count = res.Count();
                return res;
            }
        }

        public Entry GetEntry(int entryId)
        {
            throw new NotImplementedException();
        }

        public Topic GetTopic(int topicId)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Topic> GetTopics(int userId)
        {
            using (var conn = new MySqlConnection(connStr))
            {
                var lookup = new Dictionary<int, Topic>();
                var sql = @"select t.id,t.topic as name,t.date_add as updatetime,t.owner_id,e.id,e.title,e.text,e.link,e.date_add as updatetime,e.topic_id as topicid from learning_logs_topic t left join learning_logs_entry e on t.id = e.topic_id where t.owner_id=@userid";
                var list = conn.Query<Topic, Entry, Topic>(sql, (t, e) =>
                {
                    if (!lookup.TryGetValue(t.Id, out var topic))
                    {
                        topic = t;
                        topic.Entries = new List<Entry>();
                        lookup.Add(topic.Id, topic);
                    }
                    topic.Entries.Add(e);
                    return topic;
                }, new { userid = userId }, splitOn: "owner_id");
                return lookup.Values;
            }
            using (var conn = new MySqlConnection(connStr))
            {
                conn.Open();
                var res = conn.Query<Topic>("SELECT id,topic as name,date_add as updatetime,owner_id as ownerid FROM learning_logs_topic where owner_id=@userid;", new { userid = userId });
                int count = res.Count();
                return res;
            }
        }

        public int SaveEntry(Entry entry)
        {
            throw new NotImplementedException();
        }

        public int SaveTopic(Topic topic)
        {
            throw new NotImplementedException();
        }
    }
}