using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;
using System.Threading.Tasks;

namespace NetApp.Repository.Interfaces
{
    public interface IAvmtRepo
    {
        void Add(FunctionLocation functionLocation);
        void AddRange(IEnumerable<FunctionLocation> functionLocations);

        List<FunctionLocation> GetFunctionLocations(int startIndex, int pageSize);
        Task<List<FunctionLocation>> GetFunctionLocationsAsync(int startIndex, int pageSize);

        FunctionLocation FindFunctionLocation(string id, string workspaceId);

        FunctionLocation RemoveFunctionLocation(string id, string workspaceId);
        void UpdateFunctionLocation(FunctionLocation functionLocation);

        Task<List<MainTransferBill>> GetMainTransfersBillsAsync();
        Task<List<DisTransferBill>> GetDisTransfersBillsAsync();
        Task<List<ChangeBill>> GetChangeBillsAsync();
    }
}