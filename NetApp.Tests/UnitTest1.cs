using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Moq;

using NetApp.Entities;
using NetApp.Entities.LearningLog;
using NetApp.Business;
using NetApp.Business.Interfaces;
using NetApp.Business.Interfaces.DTO;
using NetApp.Repository;
using NetApp.Repository.Interfaces;

namespace NetApp.Tests
{
    [TestClass]
    public class UnitTest1
    {

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

    }
}