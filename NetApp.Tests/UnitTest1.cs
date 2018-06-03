using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NetApp.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var controller = new Controllers.SampleDataController();
            var res = controller.WeatherForecasts(0);
            Assert.IsTrue(res.Any());
        }

        [TestMethod]
        public void TestComputerVision()
        {
            string filePath = string.Empty;
            Task.Run(async () =>
            {
                using (var client = new HttpClient())
                {
                    using (var fs = File.Create("tmp.jpg"))
                    {
                        var content = await client.GetStreamAsync(@"http://193.112.41.28/downloads/Snipaste_2018-05-04_00-13-13.jpg");
                        await content.CopyToAsync(fs);
                    }
                }
            }).Wait();

            FileInfo tempfile = new FileInfo("tmp.jpg");

            Assert.IsTrue(tempfile.Exists);

            string res = string.Empty;

            var controller = new Controllers.SampleDataController();
            controller.WhatCanYouSee(tempfile.FullName).Wait();

            tempfile.Delete();
            Assert.IsTrue(!string.IsNullOrEmpty(res));
        }
    }
}
