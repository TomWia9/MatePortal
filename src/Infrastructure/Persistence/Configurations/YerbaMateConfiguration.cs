﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

/// <summary>
///     The yerba mate configuration
/// </summary>
public class YerbaMateConfiguration : IEntityTypeConfiguration<YerbaMate>
{
    /// <summary>
    ///     Configures yerba mates
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<YerbaMate> builder)
    {
        builder.Ignore(y => y.DomainEvents);
        builder.HasKey(y => y.Id);

        builder.HasMany(y => y.YerbaMateOpinions).WithOne(o => o.YerbaMate);
        builder.HasMany(y => y.Favourites).WithOne(f => f.YerbaMate);
        builder.HasMany(y => y.YerbaMateImages).WithOne(i => i.YerbaMate);

        builder.Property(y => y.Name).HasMaxLength(200).IsRequired();
        builder.Property(y => y.Description).HasMaxLength(1000).IsRequired();
        builder.Property(y => y.AveragePrice).HasColumnType("money").IsRequired();
    }
}