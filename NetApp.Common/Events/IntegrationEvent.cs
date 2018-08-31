using System;
using System.Collections.Generic;
using System.Text;

namespace NetApp.Common.Events
{
    /// <summary>
    /// An Integration Event is an event that can cause side effects to other microsrvices, Bounded-Contexts or external systems.
    /// </summary>
    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreationDate = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public DateTime CreationDate { get; }
    }
}
