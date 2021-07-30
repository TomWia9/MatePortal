using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Queries;
using Application.Opinions.Queries.GetUserYerbaMateOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Queries
{
    /// <summary>
    /// Get user's yerba mate opinions tests
    /// </summary>
    public class GetUserYerbaMateOpinionsTests : IntegrationTest
    {
        /// <summary>
        /// Get opinions should return all user's opinions
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllUserOpinions()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var response =
                await _mediator.Send(new GetUserYerbaMateOpinionsQuery(userId, new OpinionsQueryParameters()));
            response.Count.Should().Be(3);
        }

        /// <summary>
        /// Get user's yerba mate opinions with specified min and max rate should return correct count of opinions
        /// </summary>
        [Theory]
        [InlineData(1, 10, 3)]
        [InlineData(6, 6, 1)]
        [InlineData(6, 9, 2)]
        [InlineData(9, 10, 2)]
        public async Task GetUsersYerbaMateOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfOpinions(
            int minRate, int maxRate, int expectedCount)
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var response = await _mediator.Send(new GetUserYerbaMateOpinionsQuery(userId, new OpinionsQueryParameters()
                {MinRate = minRate, MaxRate = maxRate}));

            response.Count.Should().Be(expectedCount);
        }

        /// <summary>
        /// Get user's yerba mate opinions with specified search query should return correct opinions
        /// </summary>
        [Fact]
        public async Task GetUsersYerbaMateOpinionsWithSpecifiedSearchQueryShouldReturnCorrectOpinions()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var response = await _mediator.Send(new GetUserYerbaMateOpinionsQuery(userId, new OpinionsQueryParameters()
            {
                SearchQuery = "com"
            }));

            response.Count.Should().Be(2);
            response[0].Comment.Should().Be("Comment 1");
            response[1].Comment.Should().Be("Comment 2");
        }
    }
}