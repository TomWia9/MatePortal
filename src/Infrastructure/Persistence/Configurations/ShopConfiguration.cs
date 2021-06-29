using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class ShopConfiguration : IEntityTypeConfiguration<Shop>
    {
        public void Configure(EntityTypeBuilder<Shop> builder)
        {
            builder.Ignore(s => s.DomainEvents);
            builder.HasKey(s => s.Id);

            builder.HasMany(s => s.Opinions).WithOne(s => s.Shop);

            builder.Property(s => s.Name).HasMaxLength(100).IsRequired();
            builder.Property(s => s.Description).HasMaxLength(1000).IsRequired();
        }
    }
}