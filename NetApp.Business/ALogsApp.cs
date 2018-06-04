using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NetApp.Business.Interfaces;
using NetApp.Business.Interfaces.DTO;
using NetApp.Repository.Interfaces;
using NetApp.Entities.LearningLog;
using AutoMapper;

namespace NetApp.Business
{
    public class ALogsApp : ILogsApp
    {
        ILearningLogRepo learningLogRepo;

        public ALogsApp(ILearningLogRepo llRepo)
        {
            learningLogRepo = llRepo;

            Mapper.Initialize(x =>
            {
                x.CreateMap<Topic, TopicHeaderDTO>();
                x.CreateMap<Topic, TopicDTO>().ForMember(d => d.EntryHeaders, o => o.MapFrom(s => s.Entries));
                x.CreateMap<Entry, EntryHeaderDTO>();
                x.CreateMap<Entry, EntryDTO>();
            });
        }

        public EntryDTO GetEntryDetail(int entryId)
        {
            var entry = learningLogRepo.GetEntry(entryId);
            var res = Mapper.Map<EntryDTO>(entry);
            return res;
        }

        public TopicDTO GetUserTopicDetail(int topicId)
        {
            var topic = learningLogRepo.GetTopicWithEntries(topicId);
            if (topic == null)
                return null;
            var topicDTO = Mapper.Map<TopicDTO>(topic);
            return topicDTO;
        }

        public IEnumerable<TopicHeaderDTO> GetUserTopics(int userId)
        {
            if (userId < 1)
                return new TopicHeaderDTO[] { };
            var topics = learningLogRepo.GetTopics(userId);
            var res = Mapper.Map<IEnumerable<TopicHeaderDTO>>(topics);
            return res;
        }
    }
}