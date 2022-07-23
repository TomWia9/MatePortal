using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Queries;
using Application.ShopOpinions.Queries.GetShopOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Queries;

/// <summary>
///     Get all shop opinions tests
/// </summary>
public class GetShopOpinionsTests : IntegrationTest
{
    /// <summary>
    ///     Get shop opinions should return all shop opinions
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllShopOpinions()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var response =
            await Mediator.Send(new GetShopOpinionsQuery(new ShopOpinionsQueryParameters()));
        response.Count.Should().Be(3);
    }

    /// <summary>
    ///     Get shop opinions should return all shop opinions for given shop
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllOpinionsForSingleShop()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);
        var parameters = new ShopOpinionsQueryParameters
        {
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5") //one of seeded shops
        };

        var response =
            await Mediator.Send(new GetShopOpinionsQuery(parameters));
        response.Count.Should().Be(2);
    }

    /// <summary>
    ///     Get all shops opinions with specified min and max rate should return correct count of given shop opinions
    /// </summary>
    [Theory]
    [InlineData(1, 10, 2)]
    [InlineData(8, 10, 1)]
    [InlineData(10, 10, 1)]
    [InlineData(1, 2, 0)]
    public async Task GetShopOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfGivenShopOpinions(
        int minRate, int maxRate, int expectedCount)
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);
        var parameters = new ShopOpinionsQueryParameters
        {
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"), //one of seeded shops
            MinRate = minRate,
            MaxRate = maxRate
        };

        var response = await Mediator.Send(new GetShopOpinionsQuery(parameters));

        response.Count.Should().Be(expectedCount);
    }

    /// <summary>
    ///     Get shop opinions with specified search query should return correct shop opinions
    /// </summary>
    [Fact]
    public async Task GetShopOpinionsWithSpecifiedSearchQueryShouldReturnCorrectShopOpinions()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var parameters = new ShopOpinionsQueryParameters
        {
            ShopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"), //one of seeded shops
            SearchQuery = "uper"
        };

        var response = await Mediator.Send(new GetShopOpinionsQuery(parameters));

        response.Count.Should().Be(1);
        response[0].Comment.Should().Be("Super comment 3");
    }

    /// <summary>
    ///     Get shop opinions should return all user's shop opinions
    /// </summary>
    [Fact]
    public async Task ShouldReturnAllUserShopOpinions()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var parameters = new ShopOpinionsQueryParameters
        {
            UserId = userId
        };

        var response = await Mediator.Send(new GetShopOpinionsQuery(parameters));
        response.Count.Should().Be(3);
    }

    /// <summary>
    ///     Get user's shop opinions with specified min and max rate should return correct count of opinions
    /// </summary>
    [Theory]
    [InlineData(1, 10, 3)]
    [InlineData(6, 6, 1)]
    [InlineData(6, 9, 2)]
    [InlineData(9, 10, 2)]
    public async Task GetUsersShopOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfShopOpinions(
        int minRate, int maxRate, int expectedCount)
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var parameters = new ShopOpinionsQueryParameters
        {
            UserId = userId,
            MinRate = minRate,
            MaxRate = maxRate
        };

        var response = await Mediator.Send(new GetShopOpinionsQuery(parameters));

        response.Count.Should().Be(expectedCount);
    }

    /// <summary>
    ///     Get user's shop opinions with specified search query should return correct shop opinions
    /// </summary>
    [Fact]
    public async Task GetUsersShopOpinionsWithSpecifiedSearchQueryShouldReturnCorrectShopOpinions()
    {
        var userId = await AuthHelper.RunAsDefaultUserAsync(Factory);
        await TestSeeder.SeedTestShopOpinionsAsync(Factory);

        var parameters = new ShopOpinionsQueryParameters
        {
            UserId = userId,
            SearchQuery = "com"
        };

        var response = await Mediator.Send(new GetShopOpinionsQuery(parameters));

        response.Count.Should().Be(3);
        response[0].Comment.Should().Be("Comment 1");
        response[1].Comment.Should().Be("Comment 2");
        response[2].Comment.Should().Be("Super comment 3");
    }
}