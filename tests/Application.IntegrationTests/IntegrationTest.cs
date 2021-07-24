using System;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using Xunit;

namespace Application.IntegrationTests
{
    /// <summary>
    /// Abstract Integration test
    /// </summary>
    public abstract class IntegrationTest : IClassFixture<CustomWebApplicationFactory>, IDisposable
    {
        /// <summary>
        /// Database checkpoint
        /// </summary>
        private readonly Checkpoint _checkpoint = new()
        {
            TablesToIgnore = new[] {"__EFMigrationsHistory", "Countries"},
            WithReseed = true
        };

        /// <summary>
        /// The mediator
        /// </summary>
        protected readonly ISender _mediator;

        /// <summary>
        /// Custom web application factory instance
        /// </summary>
        private readonly CustomWebApplicationFactory _factory;

        /// <summary>
        /// Initializes Integration test
        /// </summary>
        /// <param name="fixture">The custom web application factory fixture</param>
        protected IntegrationTest(CustomWebApplicationFactory fixture)
        {
            _factory = fixture;
            _factory.CreateClient();

            //Creates mediator service
            var scope = _factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<ISender>();
        }

        /// <summary>
        /// Disposes integration test by reset database to default state
        /// </summary>
        public void Dispose()
        {
            _checkpoint.Reset(_factory.Configuration.GetConnectionString("MatePortalTestDbConnection")).Wait();
        }
    }
}