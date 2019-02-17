using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using NetApp.Repository;
using NetApp.Services.Lib.Attributes;

namespace NetApp.Services.Basket.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly ILogger<BasketController> _logger;
        private readonly ActionBlock<string> userInputAciont;
        private BasketDBContext _context;

        public BasketController(ILogger<BasketController> logger, BasketDBContext context)
        {
            _logger = logger;
            _context = context;
            //actionblock use a uses a task to do the executuion in parallel
            userInputAciont = new ActionBlock<string>(async s =>
            {
                var count = await _context.BasketItems.CountAsync();
                _logger.LogDebug($"someone said {s} in thread {Thread.CurrentThread.ManagedThreadId}, task {Task.CurrentId}");
            });
        }

        [MyAttribute]
        [HttpGet]
        public async Task<string> DoActionBlock(string message)
        {
            var count = await _context.BasketItems.CountAsync();
            _logger.LogDebug($"action in thread {Thread.CurrentThread.ManagedThreadId}, task {Task.CurrentId}");
            userInputAciont.Post(message);
            return await Task.FromResult("hello");
        }
    }
}