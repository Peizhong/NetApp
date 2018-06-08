using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;

using NetApp.Entities;
using NetApp.Business.Interfaces;
using NetApp.Business.Interfaces.DTO;

namespace NetApp.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LearningLogController : ControllerBase
    {
        private ILogsApp logsApp;
        private UserManager<ApplicationUser> userManager;

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager == null ? Task.FromResult(new ApplicationUser()) : userManager.GetUserAsync(HttpContext.User);

        public LearningLogController(ILogsApp lgapp, UserManager<ApplicationUser> umanager)
        {
            logsApp = lgapp;

            //todo: in unittest, don't know what does usermanager need, maybe not a good idea to use it in controller..
            userManager = umanager;
        }

        [HttpGet("[action]")]
        public async Task<IEnumerable<TopicHeaderDTO>> Topics()
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new TopicHeaderDTO[] { };
                IEnumerable<TopicHeaderDTO> res = logsApp.GetUserTopics(user.Id);
                if (!res.Any())
                {
                    return new[] { new TopicHeaderDTO { Id = 0, Name = "新建主题" } };
                }
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet("[action]/{topicId}")]
        public async Task<TopicDTO> Topic(int topicId)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new TopicDTO();
                TopicDTO res = logsApp.GetUserTopicDetail(topicId);
                if (res == null)
                    return new TopicDTO { Id = 0, Name = "新建主题", EntryHeaders = new[] { new EntryHeaderDTO { Id = 0, Title = "新建文章" } } };
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("[action]")]
        public async Task<TopicHeaderDTO> Topic([FromBody] TopicDTO topic)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new TopicHeaderDTO();
                int res = logsApp.SaveTopic(topic);
                if (res == 1)
                {
                    return topic as TopicHeaderDTO;
                }
                return new TopicHeaderDTO();
            }
            catch (Exception ex)
            {
                return new TopicHeaderDTO();
            }
        }

        [HttpGet("[action]/{entryId}")]
        public async Task<EntryDTO> Entry(int entryId)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new EntryDTO();
                EntryDTO res = logsApp.GetEntryDetail(entryId);
                if (res == null)
                    return new EntryDTO { Id = 0, Title = "新建文章" };
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpPost("[action]")]
        public async Task<EntryHeaderDTO> Entry([FromBody] EntryDTO entry)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new EntryHeaderDTO();
                int res = logsApp.SaveEntry(entry);
                if (res == 1)
                {
                    return entry as EntryHeaderDTO;
                }
                return new EntryHeaderDTO();
            }
            catch (Exception ex)
            {
                return new EntryHeaderDTO();
            }
        }
    }
}