using System.Threading.Tasks;
using Application.IntegrationTests.Helpers;
using Application.Shops.Queries;
using Application.Shops.Queries.GetShops;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Shops.Queries
{
    /// <summary>
    /// Get shops tests
    /// </summary>
    public class GetShopsTests : IntegrationTest
    {
        /// <summary>
        /// Get shops should return all shops
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllShops()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            var response = await _mediator.Send(new GetShopsQuery(new ShopsQueryParameters()));
            response.Count.Should().Be(3);
        }

        /// <summary>
        /// Get shops with specified query parameters should return correct shops
        /// </summary>
        [Fact]
        public async Task GetShopsWithSpecifiedQueryParametersShouldReturnCorrectShops()
        {
            await TestSeeder.SeedTestShopsAsync(_factory);
            
            var response = await _mediator.Send(new GetShopsQuery(new ShopsQueryParameters()
            {
                SearchQuery = "undo"
            }));
            
            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Matemundo");
        }
    }
}