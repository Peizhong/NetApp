using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Business.Interfaces
{
    public interface ILogsApp
    {
        IEnumerable<TopicHeaderDTO> GetUserTopics(string userId);
        TopicDTO GetUserTopicDetail(int topicId);
        EntryDTO GetEntryDetail(int entryId);
        int SaveEntry(EntryDTO dto);
    }
}