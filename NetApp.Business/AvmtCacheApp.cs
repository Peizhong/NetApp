using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
using NetApp.Business.Interfaces;
using NetApp.Entities.Avmt;
using NetApp.Entities.Attributes;
using NetApp.Repository.Interfaces;

namespace NetApp.Business
{
    public class AvmtCacheApp : IAvmtApp
    {
        private readonly IAvmtRepo _persistAvmt;
        private readonly IAvmtRepo _cacheAvmt;

        public bool HasCache => _cacheAvmt != null;
        public bool HasPersist => _persistAvmt != null;

        private readonly ITargetBlock<FunctionLocation> _dataInPipeline;

        public AvmtCacheApp(IEnumerable<IAvmtRepo> avmtRepo)
        {
            foreach (var repo in avmtRepo)
            {
                foreach (var attr in repo.GetType().GetCustomAttributes(false))
                {
                    if (attr is RepoAttribute repoAttribute)
                    {
                        if (repoAttribute.RepoType == RepoTypeEnum.InMemory || repoAttribute.RepoType == RepoTypeEnum.Redis)
                        {
                            _cacheAvmt = repo;
                        }
                        else if (repoAttribute.RepoType == RepoTypeEnum.MySQL || repoAttribute.RepoType == RepoTypeEnum.SQLite)
                        {
                            _persistAvmt = repo;
                        }
                    }
                }
            }
            _dataInPipeline = SetupInputPipeline();
        }

        public Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize)
        {
            return _persistAvmt.GetFunctionLocationsAsync(workspaceId, startIndex, pageSize);
        }

        public async Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId)
        {
            FunctionLocation functionLocation = await _cacheAvmt.FindFunctionLocationAsync(id, workspaceId);
            if (functionLocation == null)
            {
                functionLocation = await _persistAvmt.FindFunctionLocationAsync(id, workspaceId);
                if (functionLocation != null)
                {
                    await _cacheAvmt.ReplaceFunctionLocationAsync(functionLocation);
                }
            }
            return functionLocation;
        }

        public Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation)
        {
            Console.WriteLine($"Post {functionLocation.Id}: Thread: {Thread.CurrentThread.ManagedThreadId}, Task: {Task.CurrentId}");
            _dataInPipeline.Post(functionLocation);
            return Task.FromResult(functionLocation);
        }

        private FunctionLocation saveToCache(FunctionLocation functionLocation)
        {
            Console.WriteLine($"saveToCache {functionLocation.Id}: Thread: {Thread.CurrentThread?.ManagedThreadId}, Task: {Task.CurrentId}");
            //_cacheAvmt?.UpdateFunctionLocation(functionLocation);
            return functionLocation;
        }

        private FunctionLocation saveToPersist(FunctionLocation functionLocation)
        {
            Console.WriteLine($"saveToPersist {functionLocation.Id}: Thread: {Thread.CurrentThread?.ManagedThreadId}, Task: {Task.CurrentId}");
            //_persistAvmt?.UpdateFunctionLocation(functionLocation);
            return functionLocation;
        }

        private ITargetBlock<FunctionLocation> SetupInputPipeline()
        {
            var toCache = new TransformBlock<FunctionLocation, FunctionLocation>(f => saveToCache(f));
            var toPersist = new TransformBlock<FunctionLocation, FunctionLocation>(f => saveToPersist(f));
            var end = new ActionBlock<FunctionLocation>(f => {; });
            toCache.LinkTo(toPersist);
            toPersist.LinkTo(end);
            return toCache;
        }

        public async Task<List<BillBase>> GetBillsAsync(string userId)
        {
            var allBills = new List<BillBase>();
            var main = await _persistAvmt.GetMainTransfersBillsAsync();
            var dis = await _persistAvmt.GetDisTransfersBillsAsync();
            var change = await _persistAvmt.GetChangeBillsAsync();
            allBills.AddRange(main);
            allBills.AddRange(dis);
            allBills.AddRange(change);
            return allBills;
        }

    }
}