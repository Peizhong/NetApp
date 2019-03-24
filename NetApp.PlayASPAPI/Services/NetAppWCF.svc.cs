using NetApp.PlayASPAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace NetApp.PlayASPAPI.Services
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "NetAppWCF" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select NetAppWCF.svc or NetAppWCF.svc.cs at the Solution Explorer and start debugging.
    public class NetAppWCF : INetAppWCF
    {
        public IEnumerable<Product> Products(int startIndex, int count)
        {
            var moq = Enumerable.Range(0, count).Select(n => new Product
            {
                Id = Guid.NewGuid().ToString("N"),
                Name = $"产品{startIndex + n}",
                UpdateTime = DateTime.Now
            }).ToList();
            return moq;
        }

        public void SayHello(string message)
        {
            ;
        }

        public void UpdateProduct(Product product)
        {
            //var callback = OperationContext.Current.GetCallbackChannel<INetAppWCFCallback>();
            //callback.AckMessage($"ack {product.Id}/{product.Name}");
        }
    }
}
