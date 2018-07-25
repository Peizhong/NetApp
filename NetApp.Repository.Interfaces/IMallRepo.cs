using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using NetApp.Entities.Mall;

namespace NetApp.Repository.Interfaces
{
    public interface IMallRepo
    {
        Task<List<Product>> ProductsAsync(string key, int startIndex, int pageSize);

        Task<Product> ProductAsync(int id);

        Task AddProductAsync(Product product);

        Task AddProductsAsync(IEnumerable<Product> products);

        Task UpdateProductAsync(Product product);

        Task RemoveProductAsync(Product product);

        Task<List<Order>> OrdersAsync(string key, int startIndex, int pageSize);

        Task AddOrdersAsync(IEnumerable<Order> orders);
    }
}