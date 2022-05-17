using System;
using System.Threading.Tasks;
using Application.Brands.Commands.CreateBrand;
using Application.Brands.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
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
        /// Create brand should create brand and return brand data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldCreateBrandAndReturnBrandDto()
        {
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);

            var command = new CreateBrandCommand
            {
                CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F"), // One of seeded country
                Name = "Test",
                Description = "Test description"
            };

            var expectedResult = new BrandDto
            {
                Name = command.Name,
                Description = command.Description,
                Country = "Paraguay"
            };

            var result = await _mediator.Send(command);

            result.Should().BeOfType<BrandDto>();
            result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));

            var item = await DbHelper.FindAsync<Brand>(_factory, result.Id);

            item.CreatedBy.Should().NotBeNull();
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModified.Should().BeNull();
            item.LastModifiedBy.Should().BeNull();
        }

        /// <summary>
        /// Create brand with nonexistent country should throw not found
        /// </summary>
        [Fact]
        public void CreateBrandWithNonexistentCountryShouldThrowNotFound()
        {
            var command = new CreateBrandCommand
            {
                CountryId = Guid.Parse("69FE90F5-6734-4E6B-BA81-2E03A95BB973"), //Nonexistent country id
                Name = "Test",
                Description = "Test description"
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        /// Brand should require unique name
        /// </summary>
        [Fact]
        public async Task ShouldRequireUniqueName()
        {
            await _mediator.Send(new CreateBrandCommand
            {
                Name = "Test",
                Description = "Test description",
                CountryId = Guid.Parse("C08D5B41-C678-421B-9500-93D22004F9CF")
            });

            var command = new CreateBrandCommand
            {
                Name = "Test",
                Description = "Test description",
                CountryId = Guid.Parse("C08D5B41-C678-421B-9500-93D22004F9CF")
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<ConflictException>();
        }
    }
}