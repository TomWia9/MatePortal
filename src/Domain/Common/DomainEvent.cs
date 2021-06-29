using System;

namespace Domain.Common
{
    public abstract class DomainEvent
    {
        protected DomainEvent()
        {
            DateOccured = DateTime.UtcNow;
        }

        public bool IsPublished { get; set; }
        public DateTimeOffset DateOccured { get; } = DateTime.UtcNow;
    }
}