using System;
using Application.IntegrationTests.Helpers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests;

/// <summary>
///     Abstract integration test with 'one per test content'
/// </summary>
public abstract class IntegrationTest : IDisposable
{
    /// <summary>
    ///     Custom web application factory instance
    /// </summary>
    protected readonly CustomWebApplicationFactory _factory;

    /// <summary>
    ///     The mediator
    /// </summary>
    protected readonly ISender _mediator;

    /// <summary>
    ///     Initializes Integration test
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
    ///     Disposes integration test by deleting database
    /// </summary>
    public void Dispose()
    {
        DbHelper.DeleteDatabase(_factory);
    }
}