using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using NetApp.Entities.Avmt;
using NetApp.Business.Interfaces;

namespace NetApp.Controllers
{
    [Route("api/[controller]")]
    public class SampleDataController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IAvmtApp _avmtApp;
        private readonly ILogger<SampleDataController> _logger;
        public SampleDataController(IHttpClientFactory clientFactory, IAvmtApp avmtApp, ILogger<SampleDataController> logger)
        {
            _clientFactory = clientFactory;
            var client = _clientFactory.CreateClient();
            _avmtApp = avmtApp;
            _logger = logger;
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

        /// <summary>
        /// When a request comes in, ASP.NET takes one of its thread pool threads and assigns it to that request. 
        /// This time the request handler will call that external resource asynchronously. 
        /// This returns the request thread to the thread pool until the call to the external resource returns
        /// While the thread is in the thread pool, it’s no longer associated with that request
        /// when the external resource call returns, ASP.NET takes one of its thread pool threads and reassigns it to that request
        /// with synchronous handlers, the same thread is used for the lifetime of the request
        /// with asynchronous handlers, different threads may be assigned to the same request (at different times)
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public async Task<IActionResult> DoAsync()
        {
            Console.WriteLine($"before await thread {Thread.CurrentThread?.ManagedThreadId}, task {Task.CurrentId}");
            await Task.Run(async () =>
            {
                Console.WriteLine($"in await thread {Thread.CurrentThread?.ManagedThreadId}, task {Task.CurrentId}");
                await Task.Delay(1000);
            }).ConfigureAwait(false);

            Console.WriteLine($"after await thread{Thread.CurrentThread?.ManagedThreadId}, task{Task.CurrentId}");
            return Ok("haha");
        }

        [HttpGet("[action]")]
        public async Task<string> DoSync()
        {
            Console.WriteLine($"before await thread {Thread.CurrentThread?.ManagedThreadId}, task {Task.CurrentId}");
            Thread.Sleep(1000);

            Console.WriteLine($"after await thread{Thread.CurrentThread?.ManagedThreadId}, task{Task.CurrentId}");
            return "hhaha";
        }

        [HttpGet("[action]")]
        public async Task<string> Avmt()
        {
            using (_logger.BeginScope("sample Avmt pipeling"))
            {
                _logger.LogInformation("start");
                var functionLocations = await _avmtApp.FunctionLocations(0, 100);
                foreach (var f in functionLocations)
                {
                    await _avmtApp.UpdateFunctionLocation(f);
                }
                _logger.LogInformation("done");
                return "Ok";
            }
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<FunctionLocation>> FunctionLocations(int startIndex = 0, int pageSize = 100)
        {
            return await _avmtApp.FunctionLocations(startIndex, pageSize);
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<BillBase>> Bills()
        {
            return await _avmtApp.Bills();
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