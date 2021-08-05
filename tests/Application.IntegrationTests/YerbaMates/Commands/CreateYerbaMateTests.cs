using System;
using System.Threading.Tasks;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Commands.CreateYerbaMate;
using Application.YerbaMates.Queries;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Commands
{
    /// <summary>
    /// Create yerba mate tests
    /// </summary>
    public class CreateYerbaMateTests : IntegrationTest
    {
        /// <summary>
        /// Create yerba mate should create yerba mate and return yerba mate data transfer object
        /// </summary>
        [Fact]
        public async Task ShouldCreateYerbaMateAndReturnYerbaMateDto()
        {
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            await TestSeeder.SeedTestBrandsAsync(_factory);
            var userId = await AuthHelper.RunAsAdministratorAsync(_factory);

            var command = new CreateYerbaMateCommand()
            {
                Name = "Test",
                Description = "Test description",
                imgUrl = "https://test.com",
                AveragePrice = 12.32M,
                //NumberOfAddToFav = 0,
                CategoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"), //one of seeded categories
                BrandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"), //one of seeded brands
                
            };

            var expectedResult = new YerbaMateDto()
            {
                Name = command.Name,
                Description = command.Description,
                imgUrl = "https://test.com",
                AveragePrice = 12.32M,
                NumberOfAddToFav = 0,
                Category = new CategoryDto() //one of seeded categories
                {
                    Id = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"),
                    Name = "Herbal",
                    Description = "Herbal description"
                },
                Brand = new BrandDto() //one of seeded brands
                {
                    Id = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"),
                    Name = "Kurupi",
                    Description = "Kurupi description",
                    Country = "Paraguay"
                }
            };

            var result = await _mediator.Send(command);

            result.Should().BeOfType<YerbaMateDto>();
            result.Should().BeEquivalentTo(expectedResult, x => x.Excluding(y => y.Id));

            var item = await DbHelper.FindAsync<YerbaMate>(_factory, result.Id);

            item.CreatedBy.Should().NotBeNull();
            item.CreatedBy.Should().Be(userId);
            item.Created.Should().BeCloseTo(DateTime.Now, 1000);
            item.LastModified.Should().BeNull();
            item.LastModifiedBy.Should().BeNull();
        }

        /// <summary>
        /// Yerba Mate should require unique name
        /// </summary>
        [Fact]
        public async Task ShouldRequireUniqueName()
        {
            await TestSeeder.SeedTestBrandsAsync(_factory);
            await TestSeeder.SeedTestCategoriesAsync(_factory);
            
            var command = new CreateYerbaMateCommand()
            {
                Name = "Test",
                Description = "Test description",
                imgUrl = "https://test.com",
                AveragePrice = 12.32M,
                //NumberOfAddToFav = 0,
                CategoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"), //one of seeded categories
                BrandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"), //one of seeded brands
            };
            
            await _mediator.Send(command);

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<ConflictException>();
        }
    }
}