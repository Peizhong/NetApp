using System;
using System.Collections.Generic;
using System.Linq;
using NetApp.Entities.LearningLog;
using NetApp.Repository.Interfaces;
using Microsoft.Data.Sqlite;
using Dapper;

namespace NetApp.Repository
{
    public class SQLearningLogRepo : ILearningLogRepo
    {
        static string connStr = @"Data Source=mydev.db;";

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
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                var res = conn.Query<Entry>("SELECT id,title,text,link,date_add as updatetime,topic_id as topicid FROM learning_logs_entry where topic_id=@topicid;", new { topicid = topicId });
                int count = res.Count();
                return res;
            }
        }

        public Entry GetEntry(int entryId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                var res = conn.QueryFirstOrDefault<Entry>("SELECT id,title,text,link,date_add as updatetime,topic_id as topicid FROM learning_logs_entry where id=@entryid;", new { entryid = entryId });
                return res;
            }
        }

        public IEnumerable<Topic> GetTopics(int userId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                var res = conn.Query<Topic>("SELECT id,topic as name,date_add as updatetime,owner_id as ownerid FROM learning_logs_topic where owner_id=@userid;", new { userid = userId });
                int count = res.Count();
                return res;
            }
        }

        public Topic GetTopicWithEntries(int topicId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                conn.Open();
                Topic topic = null;
                var sql = @"select t.id,t.topic as name,t.date_add as updatetime,t.owner_id,e.id,e.title,e.text,e.link,e.date_add as updatetime,e.topic_id as topicid from learning_logs_topic t left join learning_logs_entry e on t.id = e.topic_id where t.id=@topicid";
                var list = conn.Query<Topic, Entry, Topic>(sql, (t, e) =>
                {
                    if (topic == null)
                    {
                        topic = t;
                        topic.Entries = new List<Entry>();
                    }
                    topic.Entries.Add(e);
                    return topic;
                }, new { topicid = topicId }, splitOn: "owner_id");
                return topic;
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