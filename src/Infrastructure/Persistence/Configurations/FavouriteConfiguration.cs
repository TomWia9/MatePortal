using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
///     The favourite configuration
/// </summary>
public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
{
    /// <summary>
    ///     Configures favourites
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Favourite> builder)
    {
        builder.Ignore(f => f.DomainEvents);
        builder.HasKey(f => f.Id);
    }
}