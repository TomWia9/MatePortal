using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Countries.Commands.UpdateCountry;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Countries.Commands
{
    /// <summary>
    ///     Update country tests
    /// </summary>
    public class UpdateCountryTests : IntegrationTest
    {
        /// <summary>
        ///     Update country with incorrect id should throw not found exception
        /// </summary>
        [Fact]
        public void UpdateCountryWithIncorrectIdShouldThrowNotFound()
        {
            var countryId = Guid.Empty;

            var command = new UpdateCountryCommand
            {
                CountryId = countryId,
                Name = "test"
            };

            FluentActions.Invoking(() =>
                _mediator.Send(command)).Should().Throw<NotFoundException>();
        }

        /// <summary>
        ///     Update country command should update country
        /// </summary>
        [Fact]
        public async Task UpdateCountryShouldUpdateCountry()
        {
            var countryId = Guid.Parse("A7BBB4DA-12D5-4227-B6BA-690ECF40CD86"); //one of seeded countries

            var command = new UpdateCountryCommand
            {
                CountryId = countryId,
                Name = "Updated country"
            };

            await _mediator.Send(command);

            var item = await DbHelper.FindAsync<Country>(_factory, countryId);

            item.Name.Should().Be(command.Name);
        }
    }
}