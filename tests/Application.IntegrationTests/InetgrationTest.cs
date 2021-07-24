using System;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests
{
    /// <summary>
    /// Abstract integration test with 'one per test content'
    /// </summary>
    public abstract class IntegrationTest : IDisposable
    {
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
        protected IntegrationTest()
        {
            _factory = new CustomWebApplicationFactory();
            _factory.CreateClient();

            //Creates mediator service
            var scope = _factory.Services.CreateScope();
            _mediator = scope.ServiceProvider.GetRequiredService<ISender>();
        }

        /// <summary>
        /// Disposes integration test by deleting database
        /// </summary>
        public void Dispose()
        {
            using var scope = _factory.Services.CreateScope();
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureDeleted();
            };
        }
    }
}