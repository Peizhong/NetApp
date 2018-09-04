using NetApp.EventBus.Model;
using System.Threading.Tasks;

namespace NetApp.Services.Catalog.Events
{
    public interface ICatalogIntegrationEventService
    {
        Task PublishThroughEventBusAsync(IntegrationEvent evt);

        Task SaveEventAndCatalogContextChangesAsync(IntegrationEvent evt);
    }
}