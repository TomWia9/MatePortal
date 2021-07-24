using System;
using System.Linq;
using Api;
using Application.IntegrationTests.Helpers;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Application.IntegrationTests
{
    /// <summary>
    /// Custom web application factory
    /// </summary>
    public class CustomWebApplicationFactory : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Initializes CustomWebApplicationFactory
        /// </summary>
        public CustomWebApplicationFactory()
        {
            _databaseName = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Database name
        /// </summary>
        private readonly string _databaseName;

        /// <summary>
        /// Configures web host
        /// </summary>
        /// <param name="builder">The builder</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ApplicationDbContext>));

                //Removes old db context and adds db context for tests
                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>((options, context) =>
                {
                    context.UseSqlServer(
                        $"Server=localhost\\sqlexpress;Database={_databaseName};Trusted_Connection=True;");
                });

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
                        //Values seeded only in tests
                        TestSeeder.SeedTestBrands(context).Wait();
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
}