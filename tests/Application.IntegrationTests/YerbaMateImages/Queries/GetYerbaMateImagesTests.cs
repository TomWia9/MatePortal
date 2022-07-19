using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateImages.Queries;
using Application.YerbaMateImages.Queries.GetYerbaMateImages;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMateImages.Queries;

/// <summary>
///     Get yerba mate images tests
/// </summary>
public class GetYerbaMateImagesTests : IntegrationTest
{
    /// <summary>
    ///     Get images should return all yerba mate images
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllYerbaMateImages()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateImagesAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

        var response =
            await Mediator.Send(new GetYerbaMateImagesQuery(yerbaMateId, new YerbaMateImagesQueryParameters()));
        response.Count.Should().Be(3);
    }
}