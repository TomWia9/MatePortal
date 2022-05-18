using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    /// <summary>
    ///     Domain event service
    /// </summary>
    public class DomainEventService : IDomainEventService
    {
        /// <summary>
        ///     The logger
        /// </summary>
        private readonly ILogger<DomainEventService> _logger;

        /// <summary>
        ///     Initializes DomainEventService
        /// </summary>
        /// <param name="logger">The logger</param>
        public DomainEventService(ILogger<DomainEventService> logger)
        {
            _logger = logger;
        }

        /// <summary>
        ///     Publishes domain event
        /// </summary>
        /// <param name="domainEvent">The domain event</param>
        /// <returns></returns>
        public Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
            return Task.CompletedTask; //TODO Add mediator
        }
    }
}