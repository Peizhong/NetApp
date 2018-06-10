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
        public async Task<IEnumerable<TopicDTO>> Topics()
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new TopicDTO[] { };
                IEnumerable<TopicDTO> res = logsApp.GetUserTopics(user.Id);
                if (!res.Any())
                {
                    return new TopicDTO[] { };
                }
                return res;
            }
            catch (Exception ex)
            {
                return new TopicDTO[] { };
            }
        }

        [HttpGet("[action]/{topicId}")]
        public async Task<IEnumerable<EntryDTO>> TopicEntries(int topicId)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new EntryDTO[] { };
                IEnumerable<EntryDTO> res = logsApp.GetTopicEntries(topicId);
                if (res == null)
                    return new EntryDTO[] { };
                return res;
            }
            catch (Exception ex)
            {
                return new EntryDTO[] { };
            }
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<TopicDTO>> Topic([FromBody] TopicDTO topic)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new TopicDTO[] { };
                topic.OwnerId = user.Id;
                int res = logsApp.SaveTopic(topic);
                return logsApp.GetUserTopics(user.Id);
            }
            catch (Exception ex)
            {
                return new TopicDTO[] { };
            }
        }

        [HttpPost("[action]")]
        public async Task<IEnumerable<EntryDTO>> Entry([FromBody] EntryDTO entry)
        {
            try
            {
                ApplicationUser user = await GetCurrentUserAsync();
                if (user == null)
                    return new EntryDTO[] { };
                int res = logsApp.SaveEntry(entry);
                return logsApp.GetTopicEntries(entry.TopicId);
            }
            catch (Exception ex)
            {
                return new EntryDTO[] { };
            }
        }
    }
}