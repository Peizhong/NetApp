using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Linq;
using AutoMapper;

namespace NetApp.Play.Utils
{
    public class MirgrateAvmt
    {
        [Table("dm_classify")]
        public class Classify
        {
            [Key]
            public string Id { get; set; }

            [Column("parent_classify_id")]
            public string ParentId { get; set; }

            [Column("classify_name")]
            public string Name { get; set; }

            [Column("classify_type")]
            public int ClassifyType { get; set; }

            [Column("full_name")]
            public string FullPath { get; set; }

            [Column("sort_no")]
            public double? SortNo { get; set; }

            [Column("is_show")]
            public int IsShow { get; set; }

            [Column("update_time")]
            public DateTime? UpdateTime { get; set; }

            [Column("remark")]
            public string Remark { get; set; }
        }
        
        public class AvmtContext : DbContext
        {
            public AvmtContext(DbContextOptions options) :
                base(options)
            {

            }

            public DbSet<Classify> Classifies { get; set; }
        }

        public IList<Classify> GetClassifies()
        {
            IList<Classify> classifies = null;
            var sqliteBuilder = new DbContextOptionsBuilder();
            sqliteBuilder.UseSqlite("data source=C:/Users/Peizhong/Desktop/avmt.db");
            using (var context = new AvmtContext(sqliteBuilder.Options))
            {
                try
                {
                    classifies = context.Classifies.AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    var mess = ex.Message;
                }
            }
            if (classifies != null)
            {
                var classifyToCategory = new MapperConfiguration(cfg =>
                 {
                     cfg.CreateMap<Classify, Models.Mall.Category>()
                     .ForMember(c => c.CategoryId, opt => opt.MapFrom(src => src.Id))
                     .ForMember(c => c.CategoryName, opt => opt.MapFrom(src => src.Name))
                     .ForMember(c => c.CategoryType, opt => opt.MapFrom(src => src.ClassifyType))
                     .ForMember(c => c.DataStatus, opt => opt.MapFrom(src => src.IsShow))
                     .ForMember(c=>c.Parent,opt=>opt.Ignore())
                     .ForMember(c => c.Children, opt => opt.Ignore())
                     ;

                 });
                classifyToCategory.AssertConfigurationIsValid();
                var mapper = classifyToCategory.CreateMapper();

                using (var context = new Repository.MallDbContext("Server=193.112.41.28;Database=malldb;User=root;Password=mypass;"))
                {
                    var categories = mapper.Map<IList<Models.Mall.Category>>(classifies);
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }
            }
            return classifies;
        }
    }
}
