using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
///     The shop configuration
/// </summary>
public class ShopConfiguration : IEntityTypeConfiguration<Shop>
{
    /// <summary>
    ///     Configures shops
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<Shop> builder)
    {
        builder.Ignore(s => s.DomainEvents);
        builder.HasKey(s => s.Id);

        builder.HasMany(s => s.Opinions).WithOne(s => s.Shop);

        builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
        builder.Property(s => s.Description).HasMaxLength(1000).IsRequired();
    }
}