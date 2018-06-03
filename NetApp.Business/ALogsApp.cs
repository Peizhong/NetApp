using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using NetApp.Business.Interfaces;
using NetApp.Repository.Interfaces;
using NetApp.Entities.LearningLog;

namespace NetApp.Business
{
    public class ALogsApp : ILogsApp
    {
        ILearningLogRepo learningLogRepo;

        public ALogsApp(ILearningLogRepo llRepo)
        {
            learningLogRepo = llRepo;
        }

        public IEnumerable<Topic> GetUserTopics(User user)
        {
            if (user == null || user.Id < 1)
                return null;
            //var res = learningLogRepo.GetTopics(user.Id).SelectMany(t => learningLogRepo.GetEntries(t.Id)).ToList();
            var res = learningLogRepo.GetTopics(user.Id);
            Console.Write(res.Count());
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