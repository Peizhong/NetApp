using NetApp.Common.Events;
using System.Data.Common;
using System.Threading.Tasks;

namespace NetApp.Common.Abstractions
{
    public interface IIntegrationEventLogService
    {
        Task SaveEventAsync(IntegrationEvent @event, DbTransaction transaction);
        Task MarkEventAsPublishedAsync(IntegrationEvent @event);
    }
}
