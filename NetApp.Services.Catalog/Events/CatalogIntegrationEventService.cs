using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NetApp.EventBus.Abstractions;
using NetApp.EventBus.Model;
using NetApp.Repository;
using System;
using System.Data.Common;
using System.Threading.Tasks;

namespace NetApp.Services.Catalog.Events
{
    public class CatalogIntegrationEventService : ICatalogIntegrationEventService
    {
        private readonly IEventBus _eventBus;
        private readonly IIntegrationEventLogService _eventLogService;
        private MallDBContext _repo;

        public CatalogIntegrationEventService(IEventBus eventBus,
            MallDBContext repo,
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
            //Use of an EF Core resiliency strategy when using multiple DbContexts within an explicit BeginTransaction():
            //See: https://docs.microsoft.com/en-us/ef/core/miscellaneous/connection-resiliency
            //manually invoke the execution strategy with a delegate representing everything that needs to be executed.
            //If a transient failure occurs, the execution strategy will invoke the delegate again.
            await _repo.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                var dbTransactionId = _repo.Database.CurrentTransaction?.TransactionId;
                using (var transaction = _repo.Database.BeginTransaction())
                {
                    var dbTransactionId2 = _repo.Database.CurrentTransaction.TransactionId;
                    var dbTransaction = _repo.Database.CurrentTransaction.GetDbTransaction();
                    //TODO: really?
                    await _repo.SaveChangesAsync();
                    await _eventLogService.SaveEventAsync(evt, dbTransaction);

                    transaction.Commit();
                }
            });
        }
    }
}