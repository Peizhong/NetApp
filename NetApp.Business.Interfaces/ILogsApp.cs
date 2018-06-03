using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.LearningLog;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Business.Interfaces
{
    public interface ILogsApp
    {
        IEnumerable<TopicDTO> GetUserTopics(User user);
        IEnumerable<Entry> GetUserEntries(User user);
        IEnumerable<Entry> GetTopicEntries(Topic topic);
    }
}