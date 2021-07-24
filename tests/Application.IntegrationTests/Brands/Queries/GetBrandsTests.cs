using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Brands.Queries.GetBrands;
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
        public async Task GetBrandsShouldReturn5Brands()
        {
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters()));
            response.Count.Should().Be(5);
        }

        //TODO Test cases
        /// <summary>
        /// Get brands from Paraguay should return 2 brands
        /// </summary>
        [Fact]
        public async Task GetBrandsFromParaguayShouldReturn2Brands()
        {
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters() {Country = "Paraguay"}));
            response.Count.Should().Be(2);
        }
    }
}