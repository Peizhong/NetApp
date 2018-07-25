using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.Logging;
using NetApp.Entities.Avmt;
using NetApp.Entities.Attributes;
using NetApp.Repository.Interfaces;
using System.Threading.Tasks;
using StackExchange.Redis;
using Newtonsoft.Json;

namespace NetApp.Repository
{
    [Repo(RepoTypeEnum.InMemory)]
    public class InMemoryAvmtRepo : IAvmtRepo
    {
        private readonly ILogger<InMemoryAvmtRepo> _logger;
        private readonly ConcurrentDictionary<string, FunctionLocation> _functionLocations;
        private readonly ConcurrentDictionary<string, BillBase> _bills;
        private readonly ConnectionMultiplexer _redis;

        public InMemoryAvmtRepo(ILogger<InMemoryAvmtRepo> logger)
        {
            _logger = logger;

            int numProcs = Environment.ProcessorCount;
            _functionLocations = new ConcurrentDictionary<string, FunctionLocation>(numProcs * 2, 1024);
            _bills = new ConcurrentDictionary<string, BillBase>(numProcs * 2, 1024);
            _redis = ConnectionMultiplexer.Connect("193.112.41.28:6379");
        }

        public async Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId)
        {
            FunctionLocation functionLocation = null;
            try
            {
                string key = $"fl_{id}_{workspaceId}";
                IDatabase db = _redis.GetDatabase();
                string json = await db.StringGetAsync(key);
                if (!string.IsNullOrEmpty(json))
                {
                    functionLocation = JsonConvert.DeserializeObject<FunctionLocation>(json);
                    //_functionLocations.TryGetValue(key, out FunctionLocation functionLocation);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return functionLocation;
        }

        public Task<List<BasicInfoConfig>> GetBasicInfoConfigsAsync(string baseinfoTypeId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChangeBill>> GetChangeBillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Classify>> GetClassifiesAsync(IEnumerable<string> classifyIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<DisTransferBill>> GetDisTransfersBillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<List<MainTransferBill>> GetMainTransfersBillsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<TechInfoConfig>> GetTechInfoConfigsAsync(string classifyId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Workspace>> GetWorkspacesAsync(string billId)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionLocation> RemoveFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }

        public async Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation)
        {
            try
            {
                //TODO: 最大值?
                string key = $"fl_{functionLocation.Id}_{functionLocation.WorkspaceId}";
                IDatabase db = _redis.GetDatabase();
                await db.StringSetAsync(key, JsonConvert.SerializeObject(functionLocation));
                //_functionLocations[key] = functionLocation;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message, ex);
            }
            return functionLocation;
        }
    }
}