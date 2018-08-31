using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NetApp.Common.Events;

namespace NetApp.Services.Browse.Events
{
    public interface IBrowseIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent evt);
    }
}
