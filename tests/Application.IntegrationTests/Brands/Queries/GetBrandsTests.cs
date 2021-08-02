using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Brands.Queries.GetBrands;
using Application.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Brands.Queries
{
    /// <summary>
    /// Get brands tests
    /// </summary>
    public class GetBrandsTests : IntegrationTest
    {
        /// <summary>
        /// Get brands should return 5 brands
        /// </summary>
        [Fact]
        public async Task ShouldReturn5Brands()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters()));
            response.Count.Should().Be(5);
        }

        /// <summary>
        /// Get brands from specified country should return correct brands
        /// </summary>
        [Theory]
        [InlineData("Paraguay", 2)]
        [InlineData("Argentina", 1)]
        [InlineData("Brazil", 1)]
        [InlineData("Uruguay", 1)]
        public async Task GetBrandsFromSpecifiedCountryShouldReturnCorrectBrands(string country, int expectedCount)
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters() {Country = country}));

            response.Count.Should().Be(expectedCount);

            foreach (var brand in response)
            {
                brand.Country.Should().Be(country);
            }
        }
        
        /// <summary>
        /// Get brands with specified query parameters should return correct brands
        /// </summary>
        [Fact]
        public async Task GetBrandsWithSpecifiedQueryParametersShouldReturnCorrectBrands()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters()
            {
                SearchQuery = "rup"
            }));
            
            response.Count.Should().Be(1);
            response[0].Name.Should().Be("Kurupi");
        }
    }
}