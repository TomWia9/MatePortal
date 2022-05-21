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
    protected readonly CustomWebApplicationFactory Factory;

    /// <summary>
    ///     The mediator
    /// </summary>
    protected readonly ISender Mediator;

    /// <summary>
    ///     Initializes Integration test
    /// </summary>
    protected IntegrationTest()
    {
        Factory = new CustomWebApplicationFactory();

        //Creates mediator service
        var scope = Factory.Services.CreateScope();
        Mediator = scope.ServiceProvider.GetRequiredService<ISender>();
    }

    /// <summary>
    ///     Disposes integration test by deleting database
    /// </summary>
    public void Dispose()
    {
        DbHelper.DeleteDatabase(Factory);
    }
}