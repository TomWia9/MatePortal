using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.Shops.Queries;
using Application.Shops.Queries.GetShop;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Shops.Queries;

/// <summary>
///     Get shop tests
/// </summary>
public class GetShopTests : IntegrationTest
{
    /// <summary>
    ///     Get shop command should return correct shop data transfer object
    /// </summary>
    [Fact]
    public async Task ShouldReturnCorrectShop()
    {
        await TestSeeder.SeedTestShopsAsync(Factory);

        var shopId = Guid.Parse("02F73DA0-343F-4520-AEAD-36246FA446F5"); //id of one of seeded shops

        var expectedResult = new ShopDto
        {
            Id = shopId,
            Name = "Matemundo",
            Description = "Test description 1",
            Url = "https://www.matemundo.pl/"
        };

        var response = await Mediator.Send(new GetShopQuery(shopId));

        response.Should().BeOfType<ShopDto>();
        response.Should().BeEquivalentTo(expectedResult);
    }

    /// <summary>
    ///     Get shop with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void GetShopWithIncorrectIdShouldThrowNotFound()
    {
        FluentActions.Invoking(() =>
            Mediator.Send(new GetShopQuery(Guid.Empty))).Should().ThrowAsync<NotFoundException>();
    }
}