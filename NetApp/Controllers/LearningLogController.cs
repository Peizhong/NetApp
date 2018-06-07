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

        private Task<ApplicationUser> GetCurrentUserAsync() => userManager.GetUserAsync(HttpContext.User);

        public LearningLogController(ILogsApp lgapp, UserManager<ApplicationUser> umanager)
        {
            logsApp = lgapp;
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
                    return new TopicDTO();
                return res;
            }
            catch (Exception ex)
            {
                return null;
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
                    return new EntryDTO();
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