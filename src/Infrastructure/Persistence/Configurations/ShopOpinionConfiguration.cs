using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    ///     Shop opinion configuration
    /// </summary>
    public class ShopOpinionConfiguration : IEntityTypeConfiguration<ShopOpinion>
    {
        /// <summary>
        ///     Configures shops
        /// </summary>
        /// <param name="builder">Entity type builder</param>
        public void Configure(EntityTypeBuilder<ShopOpinion> builder)
        {
            builder.Ignore(s => s.DomainEvents);
            builder.HasKey(s => s.Id);

            builder.Property(o => o.Rate).IsRequired();
            builder.Property(o => o.Comment).HasMaxLength(500).IsRequired();
        }
    }
}