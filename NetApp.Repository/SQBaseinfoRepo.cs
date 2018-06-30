using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;
using Dapper;
using NetApp.Entities.Avmt;

namespace NetApp.Repository
{
    public class BaseInfoConetext : DbContext
    {
        public BaseInfoConetext(DbContextOptions<BaseInfoConetext> options)
            : base(options)
        {
        }

        public DbSet<Classify> Classifies { get; set; }
        public DbSet<BasicInfoConfig> BasicinfoConfigs { get; set; }
        public DbSet<BasicInfoDictConfig> BasicInfoDictConfigs { get; set; }
        public DbSet<TechparamConfig> TechparamConfigs { get; set; }
        public DbSet<TechparamDictConfig> TechparamDictConfigs { get; set; }
    }

    public class SQBaseInfoRepo
    {
        private DbContextOptionsBuilder<BaseInfoConetext> dbConfig;

        public SQBaseInfoRepo(string connectionString = @"Data Source=D:\Source\Repos\Comtop\Comtop.YTH\Comtop.YTH.App\bin\Debug\DB\avmt.db;")
        {
            dbConfig = new DbContextOptionsBuilder<BaseInfoConetext>();
            dbConfig.UseSqlite(connectionString);
        }

        List<Classify> allClassifies = null;

        public List<Classify> GetAllClassifies()
        {
            if (allClassifies == null)
            {
                using (var context = new BaseInfoConetext(dbConfig.Options))
                {
                    allClassifies = context.Classifies.AsNoTracking().Where(c => c.IsShow == 1).ToList();
                }
            }
            return allClassifies;
        }

        public List<BasicInfoConfig> GetBasicInfoConfigs(string classifyId)
        {
            using (var context = new BaseInfoConetext(dbConfig.Options))
            {
                var match = from c in context.Classifies
                            join b in context.BasicinfoConfigs.AsNoTracking()
                            on c.BaseInfoTypeId equals b.BaseInfoTypeId
                            where c.Id.Equals(classifyId) && c.IsShow.Equals(1) && b.IsDisplay.Equals(1)
                            orderby b.SortNo
                            select b;
                return match.ToList();
            }
        }

        ILookup<string, BasicInfoConfig> allBasicInfoConfigs = null;
        public IEnumerable<BasicInfoConfig> GetBasicInfoConfigsWithCache(string classifyId)
        {
            if (string.IsNullOrWhiteSpace(classifyId))
                return new List<BasicInfoConfig>();
            var baseInfoTypeId = allClassifies.FirstOrDefault(c => c.Id.Equals(classifyId))?.BaseInfoTypeId;
            if (string.IsNullOrWhiteSpace(baseInfoTypeId))
                return new List<BasicInfoConfig>();
            if (allBasicInfoConfigs == null)
            {
                using (var context = new BaseInfoConetext(dbConfig.Options))
                {
                    var match = from c in context.Classifies
                                join b in context.BasicinfoConfigs.AsNoTracking()
                                on c.BaseInfoTypeId equals b.BaseInfoTypeId
                                where c.IsShow.Equals(1) && b.IsDisplay.Equals(1)
                                orderby b.SortNo
                                select b;
                    allBasicInfoConfigs = match.ToLookup(b => b.BaseInfoTypeId);
                }
            }
            return allBasicInfoConfigs[baseInfoTypeId];
        }

        Dictionary<string, List<BasicInfoConfig>> allBasicInfoConfigDict = null;
        public IEnumerable<BasicInfoConfig> GetBasicInfoConfigsWithCache2(string classifyId)
        {
            if (string.IsNullOrWhiteSpace(classifyId))
                return new List<BasicInfoConfig>();
            var baseInfoTypeId = allClassifies.FirstOrDefault(c => c.Id.Equals(classifyId))?.BaseInfoTypeId;
            if (string.IsNullOrWhiteSpace(baseInfoTypeId))
                return new List<BasicInfoConfig>();
            if (allBasicInfoConfigDict == null)
            {
                allBasicInfoConfigDict = new Dictionary<string, List<BasicInfoConfig>>();
                using (var context = new BaseInfoConetext(dbConfig.Options))
                {
                    var match = from c in context.Classifies
                                join b in context.BasicinfoConfigs.AsNoTracking()
                                on c.BaseInfoTypeId equals b.BaseInfoTypeId
                                where c.IsShow.Equals(1) && b.IsDisplay.Equals(1)
                                orderby b.SortNo
                                select b;
                    foreach(var m in match.GroupBy(b=>b.BaseInfoTypeId))
                    {
                        allBasicInfoConfigDict.Add(m.Key, m.ToList());
                    }
                }
            }
            return allBasicInfoConfigDict[baseInfoTypeId];
        }
    }
}
