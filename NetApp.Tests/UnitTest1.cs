using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;

using NetApp.Entities;
using NetApp.Entities.LearningLog;
using NetApp.Controllers;
using NetApp.Business;
using NetApp.Business.Interfaces;
using NetApp.Business.Interfaces.DTO;
using NetApp.Repository;
using NetApp.Repository.Interfaces;

using NetApp.Tests.Repository;

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
                var controller = new Controllers.SampleDataController();
                res = await controller.WhatCanYouSee(@"http://193.112.41.28/downloads/Snipaste_2018-05-04_00-13-13.jpg");
            }).Wait();
            Assert.IsTrue(!string.IsNullOrEmpty(res), "call microsoft machine learning api");
        }

        [TestMethod]
        public void TestBusinessALogsApp()
        {
            var mock = new Mock<ILearningLogRepo>();

            string userId = "test";
            mock.Setup(r => r.GetTopics(It.Is<string>(s => s == userId))).Returns(new[] { new Topic { OwnerId = userId } });
            mock.Setup(r => r.GetTopics(It.Is<string>(s => s != userId))).Returns(new Topic[] { });

            var bll = new ALogsApp(mock.Object);
            var userTopic = bll.GetUserTopics(userId);
            var noUserTopic = bll.GetUserTopics("mock");

            Assert.IsTrue(userTopic.Any());
            Assert.IsFalse(noUserTopic.Any());
        }

        [TestMethod]
        public void TestBussinessWithRepository()
        {
            var option = new DbContextOptionsBuilder<TestDbContext>();
            option.UseSqlite("Data Source=mydev.db");
            var context = new TestDbContext(option.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            var bll = new ALogsApp(new SQLearningLogRepo(connectionString: "Data Source=mydev.db"));
            int saveTopicResult = bll.SaveTopic(new TopicDTO { Id = 1, OwnerId = "testOwner", Name = "testTopic" });
            int saveEntryResult = bll.SaveEntry(new EntryDTO { Id = 1, Title = "testEntry", Text = "1", Link = "1", TopicId = 1 });
            Assert.IsTrue(saveTopicResult == 1 && saveEntryResult == 1, "save topic & entry dto to repo");

            IEnumerable<TopicDTO> topics = bll.GetUserTopics("testOwner");
            Assert.IsTrue(topics.Count() == 1, "get user topics dto from repo");

            IEnumerable<EntryDTO> entries = bll.GetTopicEntries(1);
            Assert.IsTrue(entries != null && entries.Count() == 1, "get entry dto from repo");
        }

        [TestMethod]
        public void TestLearningLogController()
        {
            ApplicationUser testUser = new ApplicationUser { Id = Guid.NewGuid().ToString(), UserName = "testUser" };

            var bll = new ALogsApp(new SQLearningLogRepo(connectionString: "Data Source=mydev.db"));
            var mockUserManager = new Mock<Microsoft.AspNetCore.Identity.UserManager<ApplicationUser>>();
            mockUserManager.Setup(r => r.GetUserAsync(It.IsAny<System.Security.Claims.ClaimsPrincipal>())).Returns(Task.FromResult(testUser));

            var controller = new LearningLogController(bll, null);

            Task.Factory.StartNew(async () =>
            {
                var topic = await controller.Topics();
                Assert.IsTrue(topic.Any() && topic.First().Name == "新建主题", "get new user's topics");
            }).Wait();
        }
    }
}