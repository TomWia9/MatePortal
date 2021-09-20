using System;
using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Brands.Queries.GetBrand;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Brands.Queries
{
    /// <summary>
    ///     Get brand tests
    /// </summary>
    public class GetBrandTests : IntegrationTest
    {
        /// <summary>
        ///     Get brand command should return correct brand data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldReturnCorrectBrand()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);

            var brandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"); //id of one of seeded brand

            var expectedResult = new BrandDto
            {
                Id = brandId,
                Name = "Kurupi",
                Description = "Kurupi description",
                Country = "Paraguay"
            };

            var response = await _mediator.Send(new GetBrandQuery(brandId));

            response.Should().BeOfType<BrandDto>();
            response.Should().BeEquivalentTo(expectedResult);
        }

        /// <summary>
        ///     Get brand with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void GetBrandWithIncorrectIdShouldThrowNotFound()
        {
            var brandId = Guid.Empty;

            FluentActions.Invoking(() =>
                _mediator.Send(new GetBrandQuery(brandId))).Should().Throw<NotFoundException>();
        }
    }
}