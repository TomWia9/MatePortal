using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    /// <summary>
    /// ApplicationDbContext interface
    /// </summary>
    public interface IApplicationDbContext
    {
        /// <summary>
        /// The brands DbSet
        /// </summary>
        DbSet<Brand> Brands { get; set; }
        
        /// <summary>
        /// The categories DbSet
        /// </summary>
        DbSet<Category> Categories { get; set; }
        
        /// <summary>
        /// The countries DbSet
        /// </summary>
        DbSet<Country> Countries { get; set; }
        
        /// <summary>
        /// User's yerba mate favorites DbSet
        /// </summary>
        DbSet<Favourite> Favourites { get; set; }
        
        /// <summary>
        /// User's yerba mate opinions DbSet
        /// </summary>
        DbSet<Opinion> Opinions { get; set; }
        
        /// <summary>
        /// The yerba mates DbSet
        /// </summary>
        DbSet<YerbaMate> YerbaMate { get; set; }
        
        /// <summary>
        /// The shops DbSet
        /// </summary>
        DbSet<Shop> Shops { get; set; }
        
        /// <summary>
        /// User's shop opinions DbSet
        /// </summary>
        DbSet<ShopOpinion> ShopOpinions { get; set; }
        
        /// <summary>
        /// Saves changes async
        /// </summary>
        /// <param name="cancellationToken">The cancellation token</param>
        /// <returns>The task result of int</returns>
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);
    }
}