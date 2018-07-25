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
        private AvmtDbContext _avmtContext;

        public SQBaseInfoRepo()
        {
            var dbConfig = new DbContextOptionsBuilder<AvmtDbContext>();
            string connectionString = @"Data Source=avmt.db";
            dbConfig.UseSqlite(connectionString);
            _avmtContext = new AvmtDbContext(dbConfig.Options);
        }

        public async Task<List<MainTransferBill>> GetMainTransfersBillsAsync()
        {
            List<MainTransferBill> bills = await _avmtContext.MainTransferBills.AsNoTracking().ToListAsync();
            foreach (var m in bills)
            {
                m.Workspaces = await _avmtContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == m.Id).ToListAsync();
            }
            return bills;
        }

        public async Task<List<DisTransferBill>> GetDisTransfersBillsAsync()
        {
            List<DisTransferBill> bills = await _avmtContext.DisTransferBills.AsNoTracking().ToListAsync();
            foreach (var d in bills)
            {
                d.Workspaces = await _avmtContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == d.Id).ToListAsync();
            }
            return bills;
        }

        public async Task<List<ChangeBill>> GetChangeBillsAsync()
        {
            List<ChangeBill> bills = await _avmtContext.ChangeBills.AsNoTracking().ToListAsync();
            foreach (var c in bills)
            {
                c.Workspaces = await _avmtContext.Workspaces.AsNoTracking().Where(w => w.BusinessBillId == c.Id).ToListAsync();
            }
            return bills;
        }

        public Task<List<Workspace>> GetWorkspacesAsync(string billId)
        {
            return _avmtContext.Workspaces.Where(w => w.BusinessBillId == billId).ToListAsync();
        }

        public Task<List<FunctionLocation>> GetFunctionLocationsAsync(string workspaceId, int startIndex, int pageSize)
        {
            if (startIndex < 0)
                startIndex = 0;
            if (pageSize < 1)
                pageSize = 100;
            return _avmtContext.FunctionLocations.AsNoTracking().Where(f => f.WorkspaceId == workspaceId).OrderBy(f=>f.SortNo).Skip(startIndex).Take(pageSize).ToListAsync();
        }

        public Task<FunctionLocation> FindFunctionLocationAsync(string id, string workspaceId)
        {
            return _avmtContext.FunctionLocations.FindAsync(id, workspaceId);
        }

        public Task<FunctionLocation> ReplaceFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }

        public Task<FunctionLocation> RemoveFunctionLocationAsync(FunctionLocation functionLocation)
        {
            throw new NotImplementedException();
        }

        public Task<List<Classify>> GetClassifiesAsync(IEnumerable<string> classifyIds)
        {
            if (classifyIds?.Any() != true)
            {
                List<Classify> classifies = null;
                return Task.FromResult(classifies);
            }
            return _avmtContext.Classifies.AsNoTracking().Where(c => classifyIds.Contains(c.Id)).ToListAsync();
        }

        public async Task<List<BasicInfoConfig>> GetBasicInfoConfigsAsync(string baseinfoTypeId)
        {
            var basinfos = await _avmtContext.BasicinfoConfigs.Where(b => b.BaseInfoTypeId == baseinfoTypeId).ToListAsync();

            var match = await (from bc in _avmtContext.BasicinfoConfigs.AsNoTracking().Where(b => b.BaseInfoTypeId == baseinfoTypeId && b.IsDisplay == 1)
                               join dt in _avmtContext.BasicInfoDictConfigs.AsNoTracking()
                               on bc.DictionaryId equals dt.DictionaryId into leftjoin
                               orderby bc.SortNo
                               select new
                               {
                                   config = bc,
                                   dict = leftjoin
                               }).ToListAsync();
            foreach (var m in match)
            {
                m.config.BaseinfoDict = m.dict.Any() ? m.dict.OrderBy(d => d.SortNo).ToList() : null;
            }
            return match.Select(m => m.config).ToList();
        }

        public async Task<List<TechInfoConfig>> GetTechInfoConfigsAsync(string classifyId)
        {
            var techinfos = new List<TechInfoConfig>();
            var classifytechparam = await _avmtContext.ClassifyTechparamConfigs.AsNoTracking().Where(ct => ct.ClassifyId == classifyId).ToArrayAsync();
            var techparamIds = classifytechparam.Select(ct => ct.TechparamId).ToArray();
            var techparam = await _avmtContext.TechparamConfigs.AsNoTracking().Where(tp => techparamIds.Contains(tp.Id)).ToArrayAsync();
            var classifyttechparamIds = classifytechparam.Select(ct => ct.Id).ToArray();
            var techparamdict = await _avmtContext.TechparamDictConfigs.AsNoTracking().Where(td => classifyttechparamIds.Contains(td.ClassifyTechparamId)).ToArrayAsync();
            var match = from ct in classifytechparam
                        join tp in techparam
                        on ct.TechparamId equals tp.Id
                        join td in techparamdict
                        on ct.Id equals td.ClassifyTechparamId into leftjon
                        orderby ct.SortNo
                        select new
                        {
                            ct,
                            tp,
                            dict = leftjon
                        };
            foreach (var m in match)
            {
                var techinfoConfig = new TechInfoConfig
                {
                    Id = m.ct.Id,
                    TechparamName = m.tp.TechparamName,
                    ColumnName = m.ct.ColumnName,
                    DataType = m.ct.DataType,
                    DataLength = m.tp.DataLength,
                    DataPrecision = m.tp.DataPrecision,
                    SortNo = m.ct.SortNo,
                    TechinfoDict = m.dict.Any() ? m.dict.OrderBy(d => d.Id).ToList() : null
                };
                techinfos.Add(techinfoConfig);
            }
            return techinfos;
        }
    }
}