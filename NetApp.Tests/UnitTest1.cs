using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

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
    }
}
