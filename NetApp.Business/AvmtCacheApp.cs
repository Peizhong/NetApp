using System;
using System.Collections.Generic;
using System.Text;
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

        public bool HasCache => _cacheAvmt != null;
        public bool HasPersist => _persistAvmt != null;

        public Task<IEnumerable<FunctionLocation>> AllFunctionLocations()
        {
            var functionLocations = _persistAvmt.GetAllFunctionLocations();
            return Task.FromResult(functionLocations);
        }

        public Task<FunctionLocation> FindFunctionLocation(string id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFunctionLocation(FunctionLocation functionLocation)
        {
            Console.WriteLine($"Post {functionLocation.Id}: Thread: {Thread.CurrentThread.ManagedThreadId}, Task: {Task.CurrentId}");
            _dataInPipeline.Post(functionLocation);
            return Task.FromResult(true);
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
            var end = new ActionBlock<FunctionLocation>(null);
            toCache.LinkTo(toPersist);
            toPersist.LinkTo(end);
            return toCache;
        }
    }
}