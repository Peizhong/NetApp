using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;

namespace NetApp.Repository.Interfaces
{
    public interface IAvmtRepo
    {
        void Add(FunctionLocation functionLocation);
        void AddRange(IEnumerable<FunctionLocation> functionLocations);
        IEnumerable<FunctionLocation> GetAllFunctionLocations();
        FunctionLocation FindFunctionLocation(string id, string workspaceId);
        FunctionLocation RemoveFunctionLocation(string id, string workspaceId);
        void UpdateFunctionLocation(FunctionLocation functionLocation);
    }
}