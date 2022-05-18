using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    /// <summary>
    ///     The category configuration
    /// </summary>
    public class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        /// <summary>
        ///     Configures categories
        /// </summary>
        /// <param name="builder">Entity type builder</param>
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.Ignore(c => c.DomainEvents);
            builder.HasKey(c => c.Id);

            builder.HasMany(c => c.YerbaMate).WithOne(y => y.Category);

            builder.Property(c => c.Name).HasMaxLength(50).IsRequired();
            builder.Property(c => c.Description).HasMaxLength(1000).IsRequired();
        }
    }
}