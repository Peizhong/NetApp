using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.LearningLog;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Business.Interfaces
{
    public interface ILogsApp
    {
        IEnumerable<TopicHeaderDTO> GetUserTopics(int userId);
        TopicDTO GetUserTopicDetail(int topicId);
        EntryDTO GetEntryDetail(int entryId);
    }
}