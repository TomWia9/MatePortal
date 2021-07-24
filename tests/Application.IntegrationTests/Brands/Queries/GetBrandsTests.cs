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
        public GetBrandsTests(CustomWebApplicationFactory fixture) : base(fixture)
        {
        }

        /// <summary>
        /// Get brands should return 5 brands
        /// </summary>
        [Fact]
        public async Task GetBrandsShouldReturn5Brands()
        {
            var response = await _mediator.Send(new GetBrandsQuery(new BrandsQueryParameters()));
            response.Count.Should().Be(5);
        }
    }
}