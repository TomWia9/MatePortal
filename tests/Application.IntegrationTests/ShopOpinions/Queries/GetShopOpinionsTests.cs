using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Queries;
using Application.ShopOpinions.Queries.GetShopOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Queries
{
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
            await TestSeeder.SeedTestShopsAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);
            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

            var response =
                await _mediator.Send(new GetShopOpinionsQuery(shopId, new ShopOpinionsQueryParameters()));
            response.Count.Should().Be(2);
        }

        /// <summary>
        ///     Get all shops opinions with specified min and max rate should return correct count of opinions
        /// </summary>
        [Theory]
        [InlineData(1, 10, 2)]
        [InlineData(8, 10, 1)]
        [InlineData(10, 10, 1)]
        [InlineData(1, 2, 0)]
        public async Task GetShopOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfShopOpinions(
            int minRate, int maxRate, int expectedCount)
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);
            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

            var response = await _mediator.Send(new GetShopOpinionsQuery(shopId,
                new ShopOpinionsQueryParameters {MinRate = minRate, MaxRate = maxRate}));

            response.Count.Should().Be(expectedCount);
        }

        /// <summary>
        ///     Get shop opinions with specified search query should return correct shop opinions
        /// </summary>
        [Fact]
        public async Task GetShopOpinionsWithSpecifiedSearchQueryShouldReturnCorrectShopOpinions()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);
            var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //one of seeded shops

            var response = await _mediator.Send(new GetShopOpinionsQuery(shopId, new ShopOpinionsQueryParameters
            {
                SearchQuery = "uper"
            }));

            response.Count.Should().Be(1);
            response[0].Comment.Should().Be("Super comment 3");
        }
    }
}