using NetApp.EventBus.Model;
using System.Data.Common;
using System.Threading.Tasks;

namespace NetApp.EventBus.Abstractions
{
    public interface IIntegrationEventLogService
    {
        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
        Task MarkEventAsPublishedAsync(IntegrationEvent @event);
    }
}
