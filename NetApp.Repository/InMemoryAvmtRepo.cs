using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;
using NetApp.Entities.Avmt;
using NetApp.Entities.Attributes;
using NetApp.Repository.Interfaces;

namespace NetApp.Repository
{
    [Repo(RepoTypeEnum.InMemory)]
    public class InMemoryAvmtRepo : IAvmtRepo
    {
        private readonly ConcurrentDictionary<string, FunctionLocation> _functionLocations;

        public InMemoryAvmtRepo()
        {
            int numProcs = Environment.ProcessorCount;
            _functionLocations = new ConcurrentDictionary<string, FunctionLocation>(numProcs * 2, 1024);
        }

        public void Add(FunctionLocation functionLocation)
        {
            _functionLocations.TryAdd($"{functionLocation.Id}_{functionLocation.WorkspaceId}", functionLocation);
        }

        public void AddRange(IEnumerable<FunctionLocation> functionLocations)
        {
            foreach (var f in functionLocations)
            {
                _functionLocations.TryAdd($"{f.Id}_{f.WorkspaceId}", f);
            }
        }

        public FunctionLocation FindFunctionLocation(string id, string workspaceId)
        {
            _functionLocations.TryGetValue($"{id}_{workspaceId}", out FunctionLocation f);
            return f;
        }

        public IEnumerable<ChangeBill> GetChangeBills()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<DisTransferBill> GetDisTransfersBills()
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FunctionLocation> GetFunctionLocations(int startIndex, int pageSize)
        {
            return _functionLocations.Values;
        }

        public IEnumerable<MainTransferBill> GetMainTransfersBills()
        {
            throw new NotImplementedException();
        }

        public FunctionLocation RemoveFunctionLocation(string id, string workspaceId)
        {
            _functionLocations.TryRemove($"{id}_{workspaceId}", out FunctionLocation f);
            return f;
        }

        public void UpdateFunctionLocation(FunctionLocation functionLocation)
        {
            _functionLocations[$"{functionLocation.Id}_{functionLocation.WorkspaceId}"] = functionLocation;
        }
    }
}