using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestComputerVision()
        {
            string res = string.Empty;
            Task.Run(async () =>
            {
                var controller = new Controllers.SampleDataController(null);
                res = await controller.WhatCanYouSee(@"http://193.112.41.28/downloads/Snipaste_2018-05-04_00-13-13.jpg");
            }).Wait();
            Assert.IsTrue(!string.IsNullOrEmpty(res), "call microsoft machine learning api");
        }

        [TestMethod]
        public void TestGetData()
        {
            var repo = new Repository.MQLearningLogRepo();
            var app = new Business.ALogsApp(repo);
            var controller = new Controllers.SampleDataController(app);
            var res = controller.UserTopics(1);
            Assert.IsTrue(res.Count() > 0, "query mysql for learning_log");
            var topicDto = controller.TopicDetail(1);
            Assert.IsTrue(topicDto != null && topicDto.EntryHeaders.Any(), "get topic with entries, using dto");
            var entryDto = controller.EntryDetail(1);
            Assert.IsTrue(!string.IsNullOrEmpty(entryDto.Title), "get entry detail");
        }
    }
}