using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using NetApp.Business.Interfaces;

namespace NetApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAvmtApp _avmtApp;
        private readonly ILogger<SampleDataController> _logger;
        public SampleDataController(IHttpClientFactory clientFactory,IAvmtApp avmtApp, ILogger<SampleDataController> logger)
        {
            _clientFactory = clientFactory;
            var client = _clientFactory.CreateClient();
            _avmtApp = avmtApp;
            _logger = logger;

            _logger.LogInformation(1, "aa");
        }

        private static string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        [HttpGet("[action]")]
        public IEnumerable<WeatherForecast> WeatherForecasts(int startDateIndex)
        {
            var head = Request.Headers;
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

        [HttpGet("[action]")]
        public async Task<string> Avmt()
        {
            using (_logger.BeginScope("sample Avmt pipeling"))
            {
                _logger.LogInformation("start");
                var functionLocations = await _avmtApp.AllFunctionLocations();
                foreach (var f in functionLocations)
                {
                    await _avmtApp.UpdateFunctionLocation(f);
                }
                _logger.LogInformation("done");
                return "Ok";
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