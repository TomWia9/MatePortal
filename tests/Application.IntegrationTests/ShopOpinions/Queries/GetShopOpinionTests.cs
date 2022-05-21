using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.ShopOpinions.Queries;
using Application.ShopOpinions.Queries.GetShopOpinion;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.ShopOpinions.Queries
{
    /// <summary>
    ///     Get single shop opinion tests
    /// </summary>
    public class GetShopOpinionTests : IntegrationTest
    {
        /// <summary>
        ///     Get shop opinion command should return correct shop opinion data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldReturnCorrectShopOpinion()
        {
            await TestSeeder.SeedTestShopOpinionsAsync(_factory);

            var shopOpinionId = Guid.Parse("A0EDB43D-5195-4458-8C4B-8F6F9FD7E5C9"); //id of one of seeded shop opinion

            var expectedResult = new ShopOpinionDto
            {
                Id = shopOpinionId,
                Rate = 10,
                Comment = "Comment 1"
            };

            var response = await _mediator.Send(new GetShopOpinionQuery(shopOpinionId));

            response.Should().BeOfType<ShopOpinionDto>();
            response.Id.Should().Be(shopOpinionId);
            response.Rate.Should().Be(expectedResult.Rate);
            response.Comment.Should().Be(expectedResult.Comment);
        }

        /// <summary>
        ///     Get shop opinion with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void GetShopOpinionWithIncorrectIdShouldThrowNotFound()
        {
            FluentActions.Invoking(() =>
                _mediator.Send(new GetShopOpinionQuery(Guid.Empty))).Should().ThrowAsync<NotFoundException>();
        }
    }
}