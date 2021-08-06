using System;
using System.Threading.Tasks;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application.IntegrationTests.Helpers
{
    public static class DbHelper
    {
        public static async Task<T> FindAsync<T>(CustomWebApplicationFactory factory, Guid id) where T : class
        {
            var context = GetDbContext(factory);
            return await context.FindAsync<T>(id);
        }

        public static async Task<int> GetYerbaMateOpinionsCountAsync(CustomWebApplicationFactory factory,
            Guid yerbaMateId)
        {
            var context = GetDbContext(factory);
            return await context.Opinions.CountAsync(o => o.YerbaMateId == yerbaMateId);
        }
        
        public static async Task<int> GetYerbaMateAddToFavouritesCountAsync(CustomWebApplicationFactory factory,
            Guid yerbaMateId)
        {
            var context = GetDbContext(factory);
            return await context.Favourites.CountAsync(f => f.YerbaMateId == yerbaMateId);
        }
        
        public static async Task<int> GetShopOpinionsCountAsync(CustomWebApplicationFactory factory,
            Guid shopId)
        {
            var context = GetDbContext(factory);
            return await context.ShopOpinions.CountAsync(s => s.ShopId == shopId);
        }
        
        /// <summary>
        /// Gets database context
        /// </summary>
        /// <param name="factory">Web application factory</param>
        /// <returns>ApplicationDbContext service</returns>
        public static ApplicationDbContext GetDbContext(CustomWebApplicationFactory factory)
        {
            var scope = factory.Services.CreateScope();
            return scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
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