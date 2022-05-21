using System;
using System.Linq;
using Api;
using Application.Common.Interfaces;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Application.IntegrationTests;

/// <summary>
///     Custom web application factory
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
{
    /// <summary>
    ///     Database name
    /// </summary>
    private readonly string _databaseName;

    /// <summary>
    ///     Initializes CustomWebApplicationFactory
    /// </summary>
    public CustomWebApplicationFactory()
    {
        _databaseName = Guid.NewGuid().ToString();
    }

    /// <summary>
    ///     Current user ID
    /// </summary>
    public Guid CurrentUserId { get; set; }

    /// <summary>
    ///     Current user role
    /// </summary>
    public string CurrentUserRole { get; set; }

    /// <summary>
    ///     Indicates whether current user has admin access
    /// </summary>
    public bool AdministratorAccess { get; set; }

    /// <summary>
    ///     Configures web host
    /// </summary>
    /// <param name="builder">The builder</param>
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            //Replace CurrentUserService
            var currentUserServiceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(ICurrentUserService));

            services.Remove(currentUserServiceDescriptor);
            services.AddTransient(_ =>
                Mock.Of<ICurrentUserService>(s =>
                    s.UserId == CurrentUserId && s.UserRole == CurrentUserRole &&
                    s.AdministratorAccess == AdministratorAccess));

            //Replace ApplicationDbContext
            var applicationDbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<ApplicationDbContext>));

            services.Remove(applicationDbContextDescriptor);
            services.AddDbContext<ApplicationDbContext>(options => { options.UseInMemoryDatabase(_databaseName); });


            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();
            {
                var scopedServices = scope.ServiceProvider;
                var context = scopedServices.GetRequiredService<ApplicationDbContext>();
                var userManager = scopedServices.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scopedServices.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
                var logger = scopedServices
                    .GetRequiredService<ILogger<CustomWebApplicationFactory>>();

                context.Database.EnsureCreatedAsync().Wait();

                try
                {
                    //Values seeded in real and tests
                    ApplicationDbContextSeed.SeedDatabase(userManager, roleManager, context)
                        .Wait();
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred seeding the " +
                                        "database with test messages. Error: {Message}", ex.Message);
                }
            }
        });
    }
}