using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.YerbaMateOpinions.Queries;
using Application.YerbaMateOpinions.Queries.GetYerbaMateOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMateOpinions.Queries;

/// <summary>
///     Get yerba mate opinions tests
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

        var response =
            await Mediator.Send(new GetYerbaMateOpinionsQuery(new YerbaMateOpinionsQueryParameters()));
        response.Count.Should().Be(3);
    }

    /// <summary>
    ///     Get opinions should return all yerba mate opinions for given yerba mate
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllYerbaMateOpinionsForGivenYerbaMate()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters()
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43") //one of seeded yerba mates
        };

        var response =
            await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));
        response.Count.Should().Be(2);
    }

    /// <summary>
    ///     Get yerba mate opinions with specified min and max rate should return correct count of opinions
    /// </summary>
    [Theory]
    [InlineData(1, 10, 2)]
    [InlineData(8, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(1, 2, 0)]
    public async Task GetYerbaMateOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfGivenYerbaMateOpinions(
        int minRate, int maxRate, int expectedCount)
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"), //one of seeded yerba mates
            MinRate = minRate,
            MaxRate = maxRate
        };

        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));

        response.Count.Should().Be(expectedCount);
    }

    /// <summary>
    ///     Get yerba mate opinions with specified search query should return correct opinions for given yerba mate
    /// </summary>
    [Fact]
    public async Task GetYerbaMateOpinionsWithSpecifiedSearchQueryShouldReturnCorrectOpinionsForGivenYerbaMate()
    {
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters
        {
            YerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"), //one of seeded yerba mates
            SearchQuery = "es"
        };
        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));

        response.Count.Should().Be(1);
        response[0].Comment.Should().Be("test");
    }

    /// <summary>
    ///     Get opinions should return all user's yerba mate opinions
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllUserYerbaMateOpinions()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters
        {
            UserId = userId
        };

        var response =
            await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));
        response.Count.Should().Be(3);
    }

    /// <summary>
    ///     Get user's yerba mate opinions with specified min and max rate should return correct count of opinions
    /// </summary>
    [Theory]
    [InlineData(1, 10, 3)]
    [InlineData(6, 6, 1)]
    [InlineData(6, 9, 2)]
    [InlineData(9, 10, 2)]
    public async Task GetUsersYerbaMateOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfOpinions(
        int minRate, int maxRate, int expectedCount)
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters
        {
            UserId = userId,
            MinRate = minRate,
            MaxRate = maxRate
        };

        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));

        response.Count.Should().Be(expectedCount);
    }

    /// <summary>
    ///     Get user's yerba mate opinions with specified search query should return correct opinions
    /// </summary>
    [Fact]
    public async Task GetUsersYerbaMateOpinionsWithSpecifiedSearchQueryShouldReturnCorrectOpinions()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestYerbaMateOpinionsAsync(Factory);

        var parameters = new YerbaMateOpinionsQueryParameters
        {
            UserId = userId,
            SearchQuery = "com"
        };

        var response = await Mediator.Send(new GetYerbaMateOpinionsQuery(parameters));

        response.Count.Should().Be(2);
        response[0].Comment.Should().Be("Comment 1");
        response[1].Comment.Should().Be("Comment 2");
    }
}