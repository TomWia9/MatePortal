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
using Microsoft.Extensions.Configuration;
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
        /// The configuration
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Configures web host
        /// </summary>
        /// <param name="builder">The builder</param>
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            // Adds json config file to Configuration
            builder.ConfigureAppConfiguration(config =>
            {
                Configuration = new ConfigurationBuilder()
                    .AddJsonFile("integrationsettings.json")
                    .Build();

                config.AddConfiguration(Configuration);
            });

            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType ==
                         typeof(DbContextOptions<ApplicationDbContext>));

                //Removes old db context and adds db context for tests
                services.Remove(descriptor);

                services.AddDbContext<ApplicationDbContext>((options, context) =>
                {
                    context.UseSqlServer(Configuration.GetConnectionString("MatePortalTestDbConnection"),
                        x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
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
                        DbHelper.SeedTestBrands(context).Wait();
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