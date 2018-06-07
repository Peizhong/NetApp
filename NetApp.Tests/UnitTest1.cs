using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
using Moq;

using NetApp.Entities.LearningLog;
using NetApp.Business;
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
        public void TestRepositorySQLite()
        {
            var option = new DbContextOptionsBuilder<TestDbContext>();
            option.UseSqlite("Data Source=mydev.db");
            var context = new TestDbContext(option.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
            var repo = new SQLearningLogRepo(connectionString: "Data Source=mydev.db");
            var topic = repo.SaveTopic(new Topic { Id = 1, OwnerId = "test" });
            var entry = repo.SaveEntry(new Entry { Id = 1, Title = "1", Text = "1", Link = "1", TopicId = 1, UpdateTime = DateTime.Now });
            Assert.IsTrue(entry > 0);
        }
    }
}