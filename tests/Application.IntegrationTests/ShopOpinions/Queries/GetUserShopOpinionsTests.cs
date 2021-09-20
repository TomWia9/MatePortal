using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Queries;
using Application.ShopOpinions.Queries.GetUserShopOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Queries
{
    /// <summary>
    ///     Get user's shop opinions tests
    /// </summary>
    public class GetUserShopOpinionsTests : IntegrationTest
    {
        /// <summary>
        ///     Get shop opinions should return all user's shop opinions
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllUserShopOpinions()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);

            var response =
                await _mediator.Send(new GetUserShopOpinionsQuery(userId, new ShopOpinionsQueryParameters()));
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
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);

            var response = await _mediator.Send(new GetUserShopOpinionsQuery(userId,
                new ShopOpinionsQueryParameters { MinRate = minRate, MaxRate = maxRate }));

            response.Count.Should().Be(expectedCount);
        }

        /// <summary>
        ///     Get user's shop opinions with specified search query should return correct shop opinions
        /// </summary>
        [Fact]
        public async Task GetUsersShopOpinionsWithSpecifiedSearchQueryShouldReturnCorrectShopOpinions()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);

            var response = await _mediator.Send(new GetUserShopOpinionsQuery(userId, new ShopOpinionsQueryParameters
            {
                SearchQuery = "com"
            }));

            response.Count.Should().Be(3);
            response[0].Comment.Should().Be("Comment 1");
            response[1].Comment.Should().Be("Comment 2");
            response[2].Comment.Should().Be("Super comment 3");
        }
    }
}