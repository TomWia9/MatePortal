﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class YerbaMateConfiguration : IEntityTypeConfiguration<YerbaMate>
    {
        public void Configure(EntityTypeBuilder<YerbaMate> builder)
        {
            builder.Ignore(y => y.DomainEvents);
            builder.HasKey(y => y.Id);

            builder.HasMany(y => y.Opinions).WithOne(o => o.YerbaMate);
            builder.HasMany(y => y.Favourites).WithOne(f => f.YerbaMate);

            builder.Property(y => y.Name).HasMaxLength(200).IsRequired();
            builder.Property(y => y.Description).HasMaxLength(1000).IsRequired();
            builder.Property(y => y.imgUrl).HasMaxLength(500);
            builder.Property(y => y.AveragePrice).HasColumnType("money").IsRequired();
            builder.Property(y => y.NumberOfAddToFav).IsRequired();
        }
    }
}