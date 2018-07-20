using System;
using System.Collections.Generic;
using System.Text;
using NetApp.Entities.Avmt;
using System.Threading.Tasks;

namespace NetApp.Business.Interfaces
{
    public interface IAvmtApp
    {
        Task<IEnumerable<FunctionLocation>> AllFunctionLocations();
        Task<FunctionLocation> FindFunctionLocation(string id);
        Task UpdateFunctionLocation(FunctionLocation functionLocation);
    }
}
