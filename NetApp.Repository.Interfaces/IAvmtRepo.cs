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
        IEnumerable<FunctionLocation> GetFunctionLocations(int startIndex, int pageSize);
        Task<IEnumerable<FunctionLocation>> GetFunctionLocationsAsync(int startIndex, int pageSize);
        FunctionLocation FindFunctionLocation(string id, string workspaceId);
        FunctionLocation RemoveFunctionLocation(string id, string workspaceId);
        void UpdateFunctionLocation(FunctionLocation functionLocation);
        
        IEnumerable<MainTransferBill> GetMainTransfersBills();
        IEnumerable<DisTransferBill> GetDisTransfersBills();
        IEnumerable<ChangeBill> GetChangeBills();
    }
}