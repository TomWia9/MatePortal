using System.Threading.Tasks;
using Domain.Common;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// DomainEvent service interface
    /// </summary>
    public interface IDomainEventService
    {
        /// <summary>
        /// Publishes domain event
        /// </summary>
        /// <param name="domainEvent">A domain event to publish</param>
        /// <returns>Task</returns>
        Task Publish(DomainEvent domainEvent);
    }
}