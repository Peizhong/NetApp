using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using NetApp.Repository;
using NetApp.Service.Models;
using NetApp.Entities.Avmt;
using NetApp.Entities.Mall;
using AutoMapper;

namespace NetApp.Service.Extensions
{
    public static class MockDataExtension
    {
        public static IApplicationBuilder MockClassify(this IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var logger = serviceScope.ServiceProvider.GetRequiredService<ILogger<Startup>>();
                var repo = new SQBaseInfoRepo();

                using (var context = serviceScope.ServiceProvider.GetRequiredService<MallContext>())
                {
                    Task.Run(async () =>
                    {
                        try
                        {
                            logger.LogInformation("try migrate mallcontext");
                            await context.Database.MigrateAsync();

                            logger.LogInformation("check Category from mallcontext");
                            if (!context.Categories.Any())
                            {
                                var classifies = await repo.GetClassifiesAsync(null);

                                var config = new MapperConfiguration(cfg =>
                                {
                                    cfg.CreateMap<Classify, Category>()
                                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.ClassifyName.Trim()))
                                    .ForMember(dest => dest.FullPath, opt => opt.MapFrom(src => src.FullName.Trim()))
                                    .ForMember(dest => dest.CategoryType, opt => opt.MapFrom(src => src.ClassifyType))
                                    .ForMember(dest => dest.ParentCategoryId, opt => opt.MapFrom(src => src.ParentClassifyId));
                                });
                                var mapper = config.CreateMapper();
                                var categories = mapper.Map<List<Classify>, IEnumerable<Category>>(classifies);

                                await context.Categories.AddRangeAsync(categories);
                                await context.SaveChangesAsync();
                            }

                            logger.LogInformation("check Product from mallcontext");
                            if (!context.Products.Any())
                            {
                                var functionLocations = await repo.GetFunctionLocationsAsync("", 0, 0);
                                var distinctFunctionLocation = functionLocations.GroupBy(f => f.Id).Select(f => f.First());

                                Func<double, decimal> three_two = (d) =>
                                {
                                    var parts = Math.Abs(d).ToString().Split('.');
                                    string three = string.Concat(parts[0].Take(3));
                                    string two = parts.Length > 1 ? string.Concat(parts[1].Take(2)) : "0";
                                    return decimal.Parse($"{three}.{two}");
                                };

                                var config = new MapperConfiguration(cfg =>
                                {
                                    cfg.CreateMap<FunctionLocation, Product>()
                                    .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.Id))
                                    .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.FlName.Trim()))
                                    .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.FullPath.Trim()))
                                    .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.ClassifyId))
                                    .ForMember(dest => dest.Price, opt => opt.MapFrom(src => three_two(src.SortNo)));
                                });
                                var mapper = config.CreateMapper();
                                var products = mapper.Map<IEnumerable<FunctionLocation>, IEnumerable<Product>>(distinctFunctionLocation);

                                await context.Products.AddRangeAsync(products);
                                await context.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            logger.LogError(ex, "An error occurred seeding the DB.");
                        }
                    }).Wait();
                    //context.Database.EnsureDeleted();
                    //var user = context.Users.FirstOrDefault();
                    //var detail = context.OrderDetails.Include(d => d.Product).Include(d => d.Order).ThenInclude(o => o.User).Where(o => o.Order.User == user).ToArray();
                    //context.Database.EnsureCreated();
                    //SeedData.Initialize(context);
                }
            }
            return app;
        }
    }
}