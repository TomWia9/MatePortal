using System.Threading;
using System.Threading.Tasks;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IApplicationDbContext
    {
        DbSet<Brand> Brands { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<Country> Countries { get; set; }
        DbSet<Favourite> Favourites { get; set; }
        DbSet<Opinion> Opinions { get; set; }
        DbSet<YerbaMate> YerbaMate { get; set; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken);

    }
}