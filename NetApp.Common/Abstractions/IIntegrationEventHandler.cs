using NetApp.Common.Events;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NetApp.Common.Abstractions
{
    /// <summary>
    /// Contravariant interface
    /// </summary>
    /// <typeparam name="TIntegrationEvent"></typeparam>
    public interface IIntegrationEventHandler<in TIntegrationEvent> : IIntegrationEventHandler
        where TIntegrationEvent : IntegrationEvent
    {
        Task Handle(TIntegrationEvent @event);
    }

    public interface IIntegrationEventHandler
    {
    }
}
