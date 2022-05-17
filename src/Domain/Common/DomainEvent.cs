using System;

namespace Domain.Common
{
    /// <summary>
    /// Domain event
    /// </summary>
    public abstract class DomainEvent
    {
        /// <summary>
        /// Initializes domain event
        /// </summary>
        protected DomainEvent()
        {
            DateOccured = DateTime.UtcNow;
        }

        /// <summary>
        /// Indicates that event is published
        /// </summary>
        public bool IsPublished { get; set; }
        
        /// <summary>
        /// Date occured
        /// </summary>
        public DateTimeOffset DateOccured { get; } = DateTime.UtcNow;
    }
}