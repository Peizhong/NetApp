using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NetApp.Entities.Avmt;
using NetApp.Entities.Attributes;
using NetApp.Repository.Interfaces;

namespace NetApp.Repository
{
    [Repo(RepoTypeEnum.SQLite)]
    public class SQBaseInfoRepo : IAvmtRepo
    {
        private AvmtDbContext _baseInfoContext;

        public SQBaseInfoRepo()
        {
            var dbConfig = new DbContextOptionsBuilder<AvmtDbContext>();
            string connectionString = @"Data Source=avmt.db";
            dbConfig.UseSqlite(connectionString);
            _baseInfoContext = new AvmtDbContext(dbConfig.Options);
        }

        public async Task<List<MainTransferBill>> GetMainTransfersBillsAsync()
        {
            List<MainTransferBill> bills = await _baseInfoContext.MainTransferBills.AsNoTracking().ToListAsync();
            foreach (var m in bills)
            {
                m.Workspaces = await _baseInfoContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == m.Id).ToListAsync();
            }
            return bills;
        }

        public async Task<List<DisTransferBill>> GetDisTransfersBillsAsync()
        {
            List<DisTransferBill> bills = await _baseInfoContext.DisTransferBills.AsNoTracking().ToListAsync();
            foreach (var d in bills)
            {
                d.Workspaces = await _baseInfoContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == d.Id).ToListAsync();
            }
            return bills;
        }

        public async Task<List<ChangeBill>> GetChangeBillsAsync()
        {
            List<ChangeBill> bills = await _baseInfoContext.ChangeBills.AsNoTracking().ToListAsync();
            foreach (var c in bills)
            {
                c.Workspaces = await _baseInfoContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == c.Id).ToListAsync();
            }
            return bills;
        }

        public Task<List<Workspace>> GetWorkspacesAsync(string billId)
        {
            return _baseInfoContext.Workspaces.Where(w => w.BusinessBillId == billId).ToListAsync();
        }

        public Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize)
        {
            if (startIndex < 0)
                startIndex = 0;
            if (pageSize < 1)
                pageSize = 100;
            return _baseInfoContext.FunctionLocations.Where(f => f.WorkspaceId == workspaceId).Skip(startIndex).Take(pageSize).ToListAsync();
        }

        public Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId)
        {
            return _baseInfoContext.FunctionLocations.FindAsync(id, workspaceId);
        }

        public Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionLocation> RemoveFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }
    }
}