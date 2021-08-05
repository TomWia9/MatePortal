using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Queries;
using Application.YerbaMates.Queries.GetYerbaMates;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Queries
{
    /// <summary>
    /// Get yerba mates tests
    /// </summary>
    public class GetYerbaMatesTests : IntegrationTest
    {
        /// <summary>
        /// Get yerba mates should return all yerba mates
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllYerbaMates()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);
            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters()));
            response.Count.Should().Be(3);
        }

        /// <summary>
        /// Get yerba mates with specified query search should return correct yerba mates
        /// </summary>
        [Fact]
        public async Task GetYerbaMatesWithSpecifiedQuerySearchShouldReturnCorrectYerbaMates()
        {
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters()
            {
                SearchQuery = "herbal"
            }));

            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Kurupi Katuava");
            response[0].Description.Should().Be("One of the best herbal yerba");
        }

        /// <summary>
        /// Get yerba mates with specified query parameters should return correct yerba mates
        /// </summary>
        [Fact]
        public async Task GetYerbaMatesWithSpecifiedQueryParametersShouldReturnCorrectYerbaMates()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestYerbaMatesAsync(_factory);

            var response = await _mediator.Send(new GetYerbaMatesQuery(new YerbaMatesQueryParameters()
            {
                Country = "Argentina",
                MaxPrice = 22M
            }));

            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Test 1");
            response[0].Description.Should().Be("Description 1");
            response[0].Brand.Name.Should().Be("Cruz de malta");
        }
    }
}