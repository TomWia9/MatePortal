using System;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Brands.Queries;
using Application.Common.Exceptions;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Brands.Commands
{
    /// <summary>
    /// Create brand tests
    /// </summary>
    public class CreateBrandTests : IntegrationTest
    {
        /// <summary>
        /// Initializes CreateBrandTests
        /// </summary>
        /// <param name="fixture">The fixture</param>
        public CreateBrandTests(CustomWebApplicationFactory fixture) : base(fixture)
        {
        }
        
        /// <summary>
        /// Create brand should create brand and return brand data transfer object
        /// </summary>
        [Fact]
        public async Task CreateBrandShouldCreateBrandAndReturnBrandDto()
        {
            var command = new CreateBrandCommand()
            {
                CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"), // One of seeded country
                Name = "Test",
                Description = "Test description"
            };

            var expectedResult = new BrandDto()
            {
                Name = command.Name,
                Description = command.Description,
                Country = "Paraguay"
            };

            var result = await _mediator.Send(command);

            result.Should().BeOfType<BrandDto>();
            result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));
        }

        /// <summary>
        /// Create brand with nonexistent country should return not found
        /// </summary>
        [Fact]
        public void CreateBrandWithNonexistentCountryShouldReturnNotFound()
        {
            var command = new CreateBrandCommand()
            {
                CountryId = Guid.Parse("69FE90F5-6734-4E6B-BA81-2E03A95BB973"), //Nonexistent country id
                Name = "Test",
                Description = "Test description"
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }
    }
}