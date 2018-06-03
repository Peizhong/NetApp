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

            Mapper.Initialize(x => x.CreateMap<Topic, TopicDTO>());
        }
        
        public IEnumerable<TopicDTO> GetUserTopics(User user)
        {
            if (user == null || user.Id < 1)
                return null;
            var topics = learningLogRepo.GetTopics(user.Id);
            var res = Mapper.Map<IEnumerable<TopicDTO>>(topics);
            return res;
        }

        public IEnumerable<Entry> GetUserEntries(User user)
        {
            if (user == null || user.Id < 1)
                return null;
            //var res = learningLogRepo.GetTopics(user.Id).SelectMany(t => learningLogRepo.GetEntries(t.Id)).ToList();
            var res = learningLogRepo.GetTopics(user.Id).SelectMany(t => t.Entries);
            Console.Write(res.Count());
            return res;
        }

        public IEnumerable<Entry> GetTopicEntries(Topic topic)
        {
            if (topic == null || topic.Id < 1)
                return null;
            var res = learningLogRepo.GetEntries(topic.Id);
            Console.Write(res.Count());
            return res;
        }
    }
}