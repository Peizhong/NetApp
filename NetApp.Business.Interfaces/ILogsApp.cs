using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Business.Interfaces
{
    public interface ILogsApp
    {
        IEnumerable<TopicDTO> GetUserTopics(string userId);
        IEnumerable<EntryDTO>  GetTopicEntries(int topicId);

        int SaveTopic(TopicDTO dto);
        int SaveEntry(EntryDTO dto);
    }
}