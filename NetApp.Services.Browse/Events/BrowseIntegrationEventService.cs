using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetApp.Common.Abstractions;
using NetApp.Common.Events;
using NetApp.Repository;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NetApp.Services.Browse.Events
{
    public class BrowseIntegrationEventService : IBrowseIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly IIntegrationEventLogService _eventLogService;
        private MQMallRepo _repo;

        public BrowseIntegrationEventService(IEventBus eventBus,
            MQMallRepo repo,
          Func<DbConnection, IIntegrationEventLogService> integrationEventLogServiceFactory)
        {
            _repo = repo;
            _eventBus = eventBus ?? throw new ArgumentNullException(nameof(eventBus));
            _eventLogService = integrationEventLogServiceFactory(repo.Database.GetDbConnection());
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt)
        {
            _eventBus.Publish(evt);
            await _eventLogService.MarkEventAsPublishedAsync(evt);
        }

        public async Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt)
        {
            await _repo.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await _repo.SaveChangesAsync();
                await _eventLogService.SaveEventAsync(evt, _repo.Database.CurrentTransaction.GetDbTransaction());
            });
        }
    }
}