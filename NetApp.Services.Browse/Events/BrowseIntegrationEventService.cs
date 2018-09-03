using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Common.Abstractions;
using NetApp.Common.Events;

namespace NetApp.Services.Browse.Events
{
    public class BrowseIntegrationEventService : IBrowseIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        public BrowseIntegrationEventService(IEventBus eventBus)
        {
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
        }

        public Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }

        public Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            throw new NotImplementedException();
        }
    }
}