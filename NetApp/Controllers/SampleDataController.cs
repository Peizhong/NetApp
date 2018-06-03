using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetApp.Entities.LearningLog;
using NetApp.Business.Interfaces;

namespace NetApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        ILogsApp logsapp;

        public SampleDataController(ILogsApp lgapp)
        {
            logsapp = lgapp;
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
        public async Task<string> WhatCanYouSee(string imgPath)
        {
            try
            {
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


        [HttpGet("[action]")]
        public IEnumerable<Topic> UserTopics(int userId)
        {
            try
            {
                var res = logsapp.GetUserTopics(new User { Id = userId });
                Console.Write(res.Count());
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Entry> UserTopicEnries(int topicId)
        {
            try
            {
                var res = logsapp.GetTopicEntries(new Topic { Id = topicId });
                Console.Write(res.Count());
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("[action]")]
        public IEnumerable<Entry> UserEntries(int userId)
        {
            try
            {
                var res = logsapp.GetUserEntries(new User { Id = userId });
                Console.Write(res.Count());
                return res;
            }
            catch (Exception ex)
            {
                return null;
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
    }
}