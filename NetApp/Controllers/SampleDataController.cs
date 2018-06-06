using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

using NetApp.Business.Interfaces;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        ILogsApp logsapp;
        UserManager<IdentityUser> userManager;

        public SampleDataController(ILogsApp lgapp, UserManager<IdentityUser> user)
        {
            logsapp = lgapp;
            userManager = user;
        }

        private Task<IdentityUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        [Authorize]
        public async Task<IEnumerable<WeatherForecast>> WeatherForecasts(int startDateIndex)
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;

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

        [HttpGet("action")]
        [Authorize]
        public async Task<string> WhoAmI()
        {
            var user = await GetCurrentUserAsync();
            var userId = user?.Id;
            return userId ?? "";
        }

        [HttpGet("[action]")]
        public IEnumerable<TopicHeaderDTO> UserTopics(int userId)
        {
            try
            {
                var res = logsapp.GetUserTopics(userId);
                Console.Write(res.Count());
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("[action]")]
        public TopicDTO TopicDetail(int topicId)
        {
            try
            {
                var res = logsapp.GetUserTopicDetail(topicId);
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("[action]")]
        public EntryDTO EntryDetail(int entryId)
        {
            try
            {
                var res = logsapp.GetEntryDetail(entryId);
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