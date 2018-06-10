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

        static ALogsApp()
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<Topic, TopicDTO>();
                x.CreateMap<TopicDTO, Topic>()
                    .ForMember(d => d.Entries, o => o.Ignore())
                    .ForMember(d => d.UpdateTime, o => o.Ignore());

                x.CreateMap<Entry, EntryDTO>();
                x.CreateMap<EntryDTO, Entry>()
                    .ForMember(d => d.UpdateTime, o => o.Ignore());
            });
        }

        public ALogsApp(ILearningLogRepo llRepo)
        {
            learningLogRepo = llRepo;
        }

        public IEnumerable<TopicDTO> GetUserTopics(string userId)
        {
            var topics = learningLogRepo.GetTopics(userId);
            var res = Mapper.Map<IEnumerable<TopicDTO>>(topics);
            return res;
        }

        public IEnumerable<EntryDTO> GetTopicEntries(int topicId)
        {
            var entries = learningLogRepo.GetEntries(topicId);
            var res = Mapper.Map<IEnumerable<EntryDTO>>(entries);
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