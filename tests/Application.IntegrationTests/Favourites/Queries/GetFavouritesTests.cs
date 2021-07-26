using System.Threading.Tasks;
using Application.Favourites.Queries;
using Application.Favourites.Queries.GetFavourites;
using Application.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Favourites.Queries
{
    /// <summary>
    /// Get favourites tests
    /// </summary>
    public class GetFavouritesTests : IntegrationTest
    {
        /// <summary>
        /// Get favourites should return all user's favourites
        /// </summary>
        [Fact]
        public async Task ShouldReturnAllUsersFavourites()
        {
            var userId = await AuthHelper.RunAsDefaultUserAsync(_factory);

            await TestSeeder.SeedTestFavouritesAsync(_factory);
            
            var response = await _mediator.Send(new GetFavouritesQuery(userId, new FavouritesQueryParameters()));
            response.Count.Should().Be(4);
        }
    }
}