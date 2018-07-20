using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;
using System.Threading;
using NetApp.Business;
using NetApp.Repository;
using NetApp.Repository.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NetApp.Tests.Business
{
    [TestClass]
    public class TestAvmtCacheApp
    {
        public TestAvmtCacheApp()
        {

        }

        [TestMethod]
        public void TestGetRepoAttribute()
        {
            var persist = new SQBaseInfoRepo();
            var cache = new InMemoryAvmtRepo();
            var app = new AvmtCacheApp(new IAvmtRepo[] { persist, cache });
            Assert.IsTrue(app.HasCache && app.HasPersist);
        }

        [TestMethod]
        public void TestSaveData()
        {
            var persist = new SQBaseInfoRepo();
            var cache = new InMemoryAvmtRepo();
            var app = new AvmtCacheApp(new IAvmtRepo[] { persist, cache });

            var functionLocations = app.AllFunctionLocations();
            functionLocations.Wait();
            var mockFunctionLocation = functionLocations.Result.FirstOrDefault();
            mockFunctionLocation.UpdateTime = DateTime.Now;
            persist.UpdateFunctionLocation(mockFunctionLocation);
        }

        [TestMethod]
        public void TestDataInPipeLine()
        {
            var persist = new SQBaseInfoRepo();
            var cache = new InMemoryAvmtRepo();
            var app = new AvmtCacheApp(new IAvmtRepo[] { persist, cache });
            var functionLocations = app.AllFunctionLocations();
            functionLocations.Wait();
            var mockFunctionLocations = functionLocations.Result;
            Assert.IsTrue(mockFunctionLocations.Any());
            foreach (var f in mockFunctionLocations)
            {
                app.UpdateFunctionLocation(f).Wait();
            };
            Thread.Sleep(10000);
        }
    }
}
