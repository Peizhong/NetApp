using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetApp.Entities.LearningLog;
using NetApp.Repository.Interfaces;
using Microsoft.Data.Sqlite;
using Dapper;

namespace NetApp.Repository
{
    public class SQLearningLogRepo : ILearningLogRepo
    {
        static string connStr;

        public SQLearningLogRepo(string connectionString = "Data Source=DB/mydev.db;")
        {
            connStr = connectionString;
        }

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
                var res = conn.Query<Entry>("SELECT id,title,text,link,updatetime,topicid FROM Entries where topicid=@topicid;", new { topicid = topicId });
                int count = res.Count();
                return res;
            }
        }

        public Entry GetEntry(int entryId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                var res = conn.QueryFirstOrDefault<Entry>("SELECT id,title,text,link,updatetime,topicid FROM Entries where id=@entryid;", new { entryid = entryId });
                return res;
            }
        }

        public IEnumerable<Topic> GetTopics(string userId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                var res = conn.Query<Topic>("SELECT id,name,updatetime, ownerid FROM Topics where OwnerId=@userid;", new { userid = userId });
                int count = res.Count();
                return res;
            }
        }

        public Topic GetTopicWithEntries(int topicId)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                Topic topic = null;
                var sql = @"select t.id,t.name,t.updatetime,t.ownerid,e.id,e.title,e.text,e.link,e.updatetime,e.topicid from Topics t left join Entries e on t.id = e.topicid where t.id=@topicid";
                var list = conn.Query<Topic, Entry, Topic>(sql, (t, e) =>
                {
                    if (topic == null)
                    {
                        topic = t;
                        topic.Entries = new List<Entry>();
                    }
                    topic.Entries.Add(e);
                    return topic;
                }, new { topicid = topicId }, splitOn: "ownerid");
                return topic;
            }
        }

        public int SaveEntry(Entry entry)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                var sql = entry.Id == 0 ?
                @"replace into Entries (title,text,link,updatetime,topicid) values(@Title,@Text,@Link,@UpdateTime,@TopicId)" :
                @"replace into Entries (id,title,text,link,updatetime,topicid) values(@Id,@Title,@Text,@Link,@UpdateTime,@TopicId)";
                int result = conn.Execute(sql, entry);
                return result;
            }
        }

        public int SaveTopic(Topic topic)
        {
            using (var conn = new SqliteConnection(connStr))
            {
                var sql = topic.Id == 0 ?
                @"replace into Topics (name,updatetime,ownerid) values(@Name,@UpdateTime,@OwnerId)" :
                @"replace into Topics (id,name,updatetime,ownerid) values(@Id, @Name,@UpdateTime,@OwnerId)";

                int result = conn.Execute(sql, topic);
                return result;
            }
        }
    }
}