using System;
using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries;
using Application.YerbaMates.Queries.GetYerbaMate;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Queries;

/// <summary>
///     Get yerba mate tests
/// </summary>
public class GetYerbaMateTests : IntegrationTest
{
    /// <summary>
    ///     Get yerba mate command should return correct yerba mate data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldReturnCorrectYerbaMate()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //id of one of seeded yerba mates

        var expectedResult = new YerbaMateDto
        {
            Id = yerbaMateId,
            Name = "Kurupi Katuava",
            Description = "One of the best herbal yerba",
            ImgUrl = "test img url",
            AveragePrice = 15.21M,
            NumberOfOpinions = 0,
            NumberOfAddToFav = 0,
            Category = new CategoryDto
            {
                Id = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"),
                Name = "Herbal",
                Description = "Herbal description"
            },
            Brand = new BrandDto
            {
                Id = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"),
                Name = "Kurupi",
                Description = "Kurupi description",
                Country = "Paraguay"
            }
        };

        var response = await Mediator.Send(new GetYerbaMateQuery(yerbaMateId));

        response.Should().BeOfType<YerbaMateDto>();
        response.Should().BeEquivalentTo(expectedResult);
    }

    /// <summary>
    ///     Get yerba mate with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void GetYerbaMateWithIncorrectIdShouldThrowNotFound()
    {
        FluentActions.Invoking(() =>
            Mediator.Send(new GetYerbaMateQuery(Guid.Empty))).Should().ThrowAsync<NotFoundException>();
    }
}