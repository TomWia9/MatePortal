using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    /// Opinion configuration
    /// </summary>
    public class OpinionConfiguration : IEntityTypeConfiguration<Opinion>
    {
        /// <summary>
        /// Configures opinions
        /// </summary>
        /// <param name="builder">Entity type builder</param>
        public void Configure(EntityTypeBuilder<Opinion> builder)
        {
            builder.Ignore(o => o.DomainEvents);

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Rate).IsRequired();
            builder.Property(o => o.Comment).HasMaxLength(500).IsRequired();
        }
    }
}