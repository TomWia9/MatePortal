using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    public class FavouriteConfiguration : IEntityTypeConfiguration<Favourite>
    {
        public void Configure(EntityTypeBuilder<Favourite> builder)
        {
            builder.Ignore(f => f.DomainEvents);
            builder.HasKey(f => f.Id);

            builder.Property(f => f.YerbaMate).IsRequired();
        }
    }
}