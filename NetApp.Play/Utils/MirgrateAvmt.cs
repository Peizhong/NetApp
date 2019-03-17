using AutoMapper;
using Microsoft.EntityFrameworkCore;
using NetApp.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

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

        public class Device
        {
            public string Id { get; set; }

            public string AssetState { get; set; }

            public string FullPath { get; set; }

            public string DeviceName { get; set; }

            public string ClassifyId { get; set; }

            public string ManufacturerId { get; set; }

            public string VendorId { get; set; }

            public double? SortNo { get; set; }

            public DateTime? UpdateTime { get; set; }

            public string Remark { get; set; }
        }

        public class AvmtContext : DbContext
        {
            public AvmtContext(DbContextOptions options) :
                base(options)
            {

            }

            public DbSet<Classify> Classifies { get; set; }
            public DbSet<Device> Devices { get; set; }
        }

        public IList<Classify> GetClassifies()
        {
            IList<Classify> classifies = null;
            var sqliteBuilder = new DbContextOptionsBuilder();
            sqliteBuilder.UseSqlite("data source=C:/Users/wxyz/Desktop/avmt.db");
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
                    cfg.CreateMap<Classify, Models.Category>()
                    .ForMember(c => c.CategoryId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(c => c.CategoryName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(c => c.CategoryType, opt => opt.MapFrom(src => src.ClassifyType))
                    .ForMember(c => c.DataStatus, opt => opt.MapFrom(src => src.IsShow))
                    .ForMember(c => c.Parent, opt => opt.Ignore())
                    .ForMember(c => c.Children, opt => opt.Ignore())
                    ;

                });
                classifyToCategory.AssertConfigurationIsValid();
                var mapper = classifyToCategory.CreateMapper();

                var mysqlBuilder = new DbContextOptionsBuilder<MallDBContext>();
                mysqlBuilder.UseMySql("Server=192.168.3.19;Database=malldb;User=sqladmin;Password=123456");
                using (var context = new MallDBContext(mysqlBuilder.Options))
                {
                    var categories = mapper.Map<IList<Models.Category>>(classifies);
                    context.Categories.AddRange(categories);
                    context.SaveChanges();
                }
            }
            return classifies;
        }

        public IList<Device> GetDevices()
        {
            IList<Device> devices = null;
            var sqliteBuilder = new DbContextOptionsBuilder<AvmtContext>();
            sqliteBuilder.UseSqlite("data source=C:/Users/wxyz/Desktop/avmt.db");
            using (var context = new AvmtContext(sqliteBuilder.Options))
            {
                try
                {
                    devices = context.Devices.FromSql("select d.id, d.asset_state AssetState, f.full_path FullPath, d.device_name DeviceName, d.classify_id ClassifyID, d.Manufacturer_Id ManufacturerId,d.Vendor_id vendorId, f.sort_no SortNo, d.update_time UpdateTime, d.Remark " +
                        "from dm_function_location f, dm_fl_asset fla, dm_device d " +
                        "where f.fl_type = 3 and f.id = fla.function_location_id and f.workspace_id = fla.workspace_id and fla.asset_id = d.id and fla.workspace_id = d.workspace_id").ToList();
                    //devices = context.Devices.AsNoTracking().ToList();
                }
                catch (Exception ex)
                {
                    var mess = ex.Message;
                }
            }
            if (devices != null)
            {
                var classifyToCategory = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Device, Models.Product>()
                    .ForMember(c => c.Id, opt => opt.MapFrom(src => src.Id))
                    .ForMember(c => c.Name, opt => opt.MapFrom(src => src.DeviceName))
                    .ForMember(c => c.CategoryId, opt => opt.MapFrom(src => src.ClassifyId))
                    //.ForMember(c => c.ManufacturerId, opt => opt.MapFrom(src => src.ManufacturerId))
                    //.ForMember(c => c.VendorId, opt => opt.MapFrom(src => src.VendorId))
                    .ForMember(c => c.DataStatus, opt => opt.MapFrom(src => src.AssetState))
                    .ForMember(c => c.Price, opt => opt.MapFrom(src => src.SortNo / 1000f))
                    .ForMember(c => c.Description, opt => opt.MapFrom(src => src.FullPath))
                    .ForMember(c => c.UpdateTime, opt => opt.MapFrom(src => src.UpdateTime))
                    .ForMember(c => c.Remark, opt => opt.MapFrom(src => src.Remark))
                    .ForAllOtherMembers(opt => opt.Ignore());
                });
                classifyToCategory.AssertConfigurationIsValid();
                var mapper = classifyToCategory.CreateMapper();

                var mysqlBuilder = new DbContextOptionsBuilder<MallDBContext>();
                mysqlBuilder.UseMySql("Server=192.168.3.19;Database=malldb;User=sqladmin;Password=123456");
                using (var context = new MallDBContext(mysqlBuilder.Options))
                {
                    var products = mapper.Map<IList<Models.Product>>(devices);
                    var oneIdProducts = products.GroupBy(p => p.Id).Select(g => g.First());
                    context.Products.AddRange(oneIdProducts);
                    context.SaveChanges();
                }
            }
            return devices;
        }
    }
}