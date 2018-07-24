using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using System.Linq;
using NetApp.Entities.Avmt;
using NetApp.Entities.Attributes;
using NetApp.Repository.Interfaces;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace NetApp.Repository
{
    [Repo(RepoTypeEnum.InMemory)]
    public class InMemoryAvmtRepo : IAvmtRepo
    {
        private readonly ConcurrentDictionary<string, FunctionLocation> _functionLocations;
        private readonly ConcurrentDictionary<string, BillBase> _bills;

        public InMemoryAvmtRepo()
        {
            int numProcs = Environment.ProcessorCount;
            _functionLocations = new ConcurrentDictionary<string, FunctionLocation>(numProcs * 2, 1024);
            _bills = new ConcurrentDictionary<string, BillBase>(numProcs * 2, 1024);
        }

        public Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId)
        {
            string key = $"fl_{id}_{workspaceId}";
            _functionLocations.TryGetValue(key, out FunctionLocation functionLocation);
            return Task.FromResult(functionLocation);
        }

        public Task<List<ChangeBill>> GetChangeBillsAsync()
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

        public Task<List<Workspace>> GetWorkspacesAsync(string billId)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionLocation> RemoveFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation)
        {
            //TODO: 最大值?
            string key = $"fl_{functionLocation.Id}_{functionLocation.WorkspaceId}";
            _functionLocations[key] = functionLocation;
            return Task.FromResult(functionLocation);
        }
    }
}