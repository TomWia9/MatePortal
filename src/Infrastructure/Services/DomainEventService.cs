using System.Threading.Tasks;
using Application.Common.Interfaces;
using Domain.Common;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly ILogger<DomainEventService> _logger;

        public DomainEventService(ILogger<DomainEventService> logger)
        {
            _logger = logger;
        }

        public Task Publish(DomainEvent domainEvent)
        {
            _logger.LogInformation("Publishing domain event. Event - {event}", domainEvent.GetType().Name);
            return Task.CompletedTask; //TODO Add mediator
        }
    }
}