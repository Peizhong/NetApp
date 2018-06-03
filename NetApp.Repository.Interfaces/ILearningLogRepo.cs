using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.LearningLog;

namespace NetApp.Repository.Interfaces
{
    public interface ILearningLogRepo
    {
        IEnumerable<Topic> GetTopics(int userId);

        Topic GetTopic(int topicId);

        int SaveTopic(Topic topic);

        int DeleteTopic(int topicId);

        IEnumerable<Entry> GetEntries(int topicId);

        Entry GetEntry(int entryId);

        int SaveEntry(Entry entry);

        int DeleteEntry(int entryId);
    }
}