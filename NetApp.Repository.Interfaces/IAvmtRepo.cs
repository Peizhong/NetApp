using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;
using System.Threading.Tasks;

namespace NetApp.Repository.Interfaces
{
    public interface IAvmtRepo
    {
        Task<List<MainTransferBill>> GetMainTransfersBillsAsync();
        Task<List<DisTransferBill>> GetDisTransfersBillsAsync();
        Task<List<ChangeBill>> GetChangeBillsAsync();

        Task<List<Workspace>> GetWorkspacesAsync(string billId);

        Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize);

        Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId);

        Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation);

        Task<FunctionLocation> RemoveFunctionLocationAsync(FunctionLocation functionLocation);
    }
}