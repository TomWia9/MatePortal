using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class YerbaMateImageConfiguration: IEntityTypeConfiguration<YerbaMateImage>
{
    /// <summary>
    ///     Configures yerba mate images
    /// </summary>
    /// <param name="builder">Entity type builder</param>
    public void Configure(EntityTypeBuilder<YerbaMateImage> builder)
    {
        builder.Ignore(x => x.DomainEvents);

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Url).IsRequired().HasMaxLength(500);
    }
}