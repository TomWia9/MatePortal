using System;
using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Opinions.Queries;
using Application.Opinions.Queries.GetYerbaMateOpinions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Opinions.Queries
{
    /// <summary>
    /// Get all yerba mate opinions tests
    /// </summary>
    public class GetYerbaMateOpinionsTests : IntegrationTest
    {
        /// <summary>
        /// Get opinions should return all yerba mate opinions
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllYerbaMateOpinions()
        {
            var yerbaMateId = Guid.Parse("7B96511A-C4C8-4A47-9A7D-D5C3C6FB5EA6"); //seeded
            await TestSeeder.SeedTestOpinionsAsync(_factory);

            var response =
                await _mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId, new OpinionsQueryParameters()));
            response.Count.Should().Be(2);
        }

        /// <summary>
        /// Get all yerba mate opinions with specified min and max rate should return correct count of opinions
        /// </summary>
        [Theory]
        [InlineData(1, 10, 2)]
        [InlineData(8, 10, 1)]
        [InlineData(10, 10, 1)]
        [InlineData(1, 2, 0)]
        public async Task GetYerbaMateOpinionsWithSpecifiedMinAndMaxRateShouldReturnCorrectCountOfOpinions(
            int minRate, int maxRate, int expectedCount)
        {
            await TestSeeder.SeedTestOpinionsAsync(_factory);
            var yerbaMateId = Guid.Parse("7B96511A-C4C8-4A47-9A7D-D5C3C6FB5EA6"); //seeded

            var response = await _mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId,
                new OpinionsQueryParameters() {MinRate = minRate, MaxRate = maxRate}));

            response.Count.Should().Be(expectedCount);
        }

        /// <summary>
        /// Get yerba mate opinions with specified search query should return correct opinions
        /// </summary>
        [Fact]
        public async Task GetYerbaMateOpinionsWithSpecifiedSearchQueryShouldReturnCorrectOpinions()
        {
            await TestSeeder.SeedTestOpinionsAsync(_factory);
            var yerbaMateId = Guid.Parse("7B96511A-C4C8-4A47-9A7D-D5C3C6FB5EA6"); //seeded

            var response = await _mediator.Send(new GetYerbaMateOpinionsQuery(yerbaMateId, new OpinionsQueryParameters()
            {
                SearchQuery = "es"
            }));

            response.Count.Should().Be(1);
            response[0].Comment.Should().Be("test");
        }
    }
}