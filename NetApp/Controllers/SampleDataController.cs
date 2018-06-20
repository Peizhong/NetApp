using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace NetApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        public SampleDataController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            var client = _clientFactory.CreateClient();
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex)
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                DateFormatted = DateTime.Now.AddDays(index + startDateIndex).ToString("d"),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            });
        }

        [HttpGet("[action]")]
        public IEnumerable<LineHeadUserVO> LineHeadUsers(string bearuCode)
        {
            return Enumerable.Range(1, 10).Select(index => new LineHeadUserVO
            {
                Id = Guid.NewGuid().ToString("N"),
                User = $"用户 {index}",
                Department = $"厂家 {index}",
                UserCode = Guid.NewGuid().ToString("N")
            });
        }

        [HttpGet("[action]")]
        public async Task<string> WhatCanYouSee(string imgPath)
        {
            try
            {
                var client = _clientFactory.CreateClient();
                var res = await Portal.MachineLearning.Instance.GetComputerVisionResult(imgPath);
                if (string.IsNullOrEmpty(res))
                {
                    return string.Empty;
                }
                return res;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }

        public class WeatherForecast
        {
            public string DateFormatted { get; set; }
            public int TemperatureC { get; set; }
            public string Summary { get; set; }

            public int TemperatureF
            {
                get
                {
                    return 32 + (int)(TemperatureC / 0.5556);
                }
            }
        }

        public class LineHeadUserVO
        {
            public string Id { get; set; }

            /// <summary>
            /// 施工单位
            /// </summary>
            public string Department { get; set; }

            /// <summary>
            /// 施工人员
            /// </summary>
            public string User { get; set; }

            /// <summary>
            /// 施工人员资格编号
            /// </summary>
            public string UserCode { get; set; }
        }
    }
}