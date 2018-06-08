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
                x.CreateMap<Topic, TopicDTO>()
                    .ForMember(d => d.EntryHeaders, o => o.MapFrom(s => s.Entries));
                x.CreateMap<TopicDTO, Topic>()
                    .ForMember(d => d.Entries, o => o.Ignore())
                    .ForMember(d => d.UpdateTime, o => o.Ignore());

                x.CreateMap<Entry, EntryHeaderDTO>();
                x.CreateMap<Entry, EntryDTO>();
                x.CreateMap<EntryDTO, Entry>()
                    .ForMember(d => d.UpdateTime, o => o.Ignore());
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

        public IEnumerable<TopicHeaderDTO> GetUserTopics(string userId)
        {
            var topics = learningLogRepo.GetTopics(userId);
            var res = Mapper.Map<IEnumerable<TopicHeaderDTO>>(topics);
            return res;
        }

        public int SaveEntry(EntryDTO dto)
        {
            Entry entry = Mapper.Map<Entry>(dto);
            entry.UpdateTime = DateTime.Now;
            int res = learningLogRepo.SaveEntry(entry);
            return res;
        }

        public int SaveTopic(TopicDTO dto)
        {
            Topic topic = Mapper.Map<Topic>(dto);
            topic.UpdateTime = DateTime.Now;
            int res = learningLogRepo.SaveTopic(topic);
            return res;
        }
    }
}