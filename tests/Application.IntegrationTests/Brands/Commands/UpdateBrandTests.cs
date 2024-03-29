﻿using System;
using System.Threading.Tasks;
using Application.Brands.Commands.UpdateBrand;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.Brands.Commands;

/// <summary>
///     Update brand tests
/// </summary>
public class UpdateBrandTests : IntegrationTest
{
    /// <summary>
    ///     Update brand with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateBrandWithIncorrectIdShouldThrowNotFound()
    {
        var brandId = Guid.Empty;

        var command = new UpdateBrandCommand
        {
            BrandId = brandId,
            Name = "Kurupi",
            Description = "Kurupi description",
            CountryId = Guid.Parse("A42066F2-2998-47DC-A193-FF4C4080056F")
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update brand command should update brand
    /// </summary>
    [Fact]
    public async Task UpdateBrandShouldUpdateBrand()
    {
        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);

        await TestSeeder.SeedTestBrandsAsync(Factory);

        var brandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239");

        var command = new UpdateBrandCommand
        {
            BrandId = brandId,
            Name = "Updated name",
            Description = "Updated description",
            CountryId = Guid.Parse("68E2E690-B2F4-44AE-A21F-756922E25163") //one of seeded countries (Argentina)
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<Brand>(Factory, brandId);

        item.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
        item.CountryId.Should().Be(command.CountryId);
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
    }
}