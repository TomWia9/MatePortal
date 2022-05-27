using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateOpinions.Queries;
using Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Queries;

/// <summary>
///     Get all yerba mate opinions tests
/// </summary>
public class GetYerbaMateOpinionsTests : IntegrationTest
{
    /// <summary>
    ///     Get opinions should return all yerba mate opinions
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllYerbaMateOpinions()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

        var response =
            await Mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId, new YerbaMateOpinionsQueryParameters()));
        response.Count.Should().Be(2);
    }

    /// <summary>
    ///     Get all yerba mate opinions with specified min and max rate should return correct count of opinions
    /// </summary>
    [Theory]
    [InlineData(1, 10, 2)]
    [InlineData(8, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(1, 2, 0)]
    public async Task GetYerbaMateOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfOpinions(
        int minRate, int maxRate, int expectedCount)
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId,
            new YerbaMateOpinionsQueryParameters {MinRate = minRate, MaxRate = maxRate}));

        response.Count.Should().Be(expectedCount);
    }

    /// <summary>
    ///     Get yerba mate opinions with specified search query should return correct opinions
    /// </summary>
    [Fact]
    public async Task GetYerbaMateOpinionsWithSpecifiedSearchQueryShouldReturnCorrectOpinions()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);
        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId, new YerbaMateOpinionsQueryParameters
        {
            SearchQuery = "es"
        }));

        response.Count.Should().Be(1);
        response[0].Comment.Should().Be("test");
    }
}