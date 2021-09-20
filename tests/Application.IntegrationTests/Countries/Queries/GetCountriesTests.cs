using System.Linq;
using System.Threading.Tasks;
using Application.Countries.Queries.GetCountries;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Queries
{
    /// <summary>
    ///     Get countries tests
    /// </summary>
    public class GetCountries : IntegrationTest
    {
        /// <summary>
        ///     Get countries should return 4 countries
        /// </summary>
        [Fact]
        public async Task ShouldReturn4Countries()
        {
            var response = await _mediator.Send(new GetCountriesQuery());
            response.Count().Should().Be(4);
        }
    }
}