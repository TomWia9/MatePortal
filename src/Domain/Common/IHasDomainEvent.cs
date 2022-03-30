using System.Collections.Generic;

namespace Domain.Common
{
    /// <summary>
    /// IHasDomainEvent interface
    /// </summary>
    public interface IHasDomainEvent
    {
        /// <summary>
        /// Domain events list
        /// </summary>
        public List<DomainEvent> DomainEvents { get; set; }
    }
}