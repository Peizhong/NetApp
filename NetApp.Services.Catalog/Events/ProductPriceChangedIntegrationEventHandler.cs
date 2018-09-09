using NetApp.EventBus.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NetApp.Services.Catalog.Events
{
    public class ProductPriceChangedIntegrationEventHandler : IIntegrationEventHandler<ProductPriceChangedIntegrationEvent>
    {
        public ProductPriceChangedIntegrationEventHandler()
        {
        }

        public Task Handle(ProductPriceChangedIntegrationEvent @event)
        {
            Console.WriteLine("hello");
            return Task.FromResult(true);
        }
    }
}