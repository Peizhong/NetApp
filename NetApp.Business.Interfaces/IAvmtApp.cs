using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;
using System.Threading.Tasks;

namespace NetApp.Business.Interfaces
{
    public interface IAvmtApp
    {
        Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize);

        Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId);

        Task LoadFunctionLocationDetail(FunctionLocation functionLocation);

        Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation);

        Task<List<BillBase>> GetBillsAsync(string userId);

        Task<List<Classify>> GetClassifiesAsync(IEnumerable<string> classifyIds);
    }
}