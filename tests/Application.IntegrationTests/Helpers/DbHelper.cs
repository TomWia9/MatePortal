using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    public static class DbHelper
    {
        public static async Task<T> FindAsync<T>(CustomWebApplicationFactory factory, Guid id) where T : class
        {
            using var scope = factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

            if (context == null)
                throw new Exception("Failed to get ApplicationDbContext service");

            return await context.FindAsync<T>(id);
        }

        public static void DeleteDatabase(CustomWebApplicationFactory factory)
        {
            using var scope = factory.Services.CreateScope();
            var scopedServices = scope.ServiceProvider;
            var context = scopedServices.GetRequiredService<ApplicationDbContext>();
            context.Database.EnsureDeleted();
        }
    }
}