using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NetApp.Entities.Mall;

namespace NetApp.Service.Models
{
    public static class SeedData
    {
        public static void Initialize(MallContext context)
        {
            Random random = new Random(DateTime.Now.Second);
            List<Product> products = context.Products.ToList();
            if (!context.Products.Any())
            {
                products = Enumerable.Range(1, 100000).Select(n => new Product
                {
                    ProductName = $"Product {n}",
                    Description = $"Description {n}_{n}",
                    Price = (decimal)(300 * random.NextDouble())
                }).ToList();
                context.Products.AddRange(products);
                context.SaveChanges();
            }
            List<User> users = context.Users.ToList();
            if (!context.Users.Any())
            {
                users = Enumerable.Range(600, 20000).Select(n => new User
                {
                    UserName = $"User {n}",
                }).ToList();
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            //if (oneOrder == null)
            {
                int index = 0;
                foreach (var user in users)
                {
                    index++;
                    var orders = Enumerable.Range(1, random.Next(5, 20)).Select(n1 => new Order { User = user }).ToList();
                    foreach (var order in orders)
                    {
                        var details = Enumerable.Range(1, random.Next(3, 10)).Select(n2 => new OrderDetail { Order = order }).ToList();
                        foreach (var detail in details)
                        {
                            int rd = random.Next(0, products.Count - 1);
                            detail.Product = products[rd];
                            detail.Quantity = random.Next(1, 10);
                        }
                        order.TransactionId = Guid.NewGuid().ToString("N");
                        order.TotalPrice = details.Sum(d => d.Product.Price * d.Quantity);
                        order.ActualPrice = order.TotalPrice - (decimal)random.NextDouble() * 20;
                        order.OrderDetails = details;
                        order.CreateTime = DateTime.Now;
                    }
                    context.Orders.AddRange(orders);
                    if (index % 600== 0)
                    {
                        context.SaveChanges();
                    }
                }
                context.SaveChanges();
            }
        }
    }
}
