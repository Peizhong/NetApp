using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace NetApp.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            var ctl = new Controllers.SampleDataController();
            var data = ctl.WeatherForecasts(1);
            Assert.IsTrue(data.Any());
        }
    }
}
