using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class BrandConfiguration : IEntityTypeConfiguration<Brand>
    {
        public void Configure(EntityTypeBuilder<Brand> builder)
        {
            builder.Ignore(b => b.DomainEvents);

            builder.HasKey(b => b.Id);

            builder.HasMany(b => b.YerbaMate).WithOne(y => y.Brand);

            builder.Property(b => b.Name).HasMaxLength(60).IsRequired();
            builder.Property(b => b.Description).HasMaxLength(1000).IsRequired();
        }
    }
}