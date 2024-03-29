﻿using System;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Application.YerbaMates.Commands.UpdateYerbaMate;
using Domain.Entities;
using FluentAssertions;
using FluentAssertions.Extensions;
using Xunit;

namespace Application.IntegrationTests.YerbaMates.Commands;

/// <summary>
///     Update yerba mate tests
/// </summary>
public class UpdateYerbaMateTests : IntegrationTest
{
    /// <summary>
    ///     Update yerba mate with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void UpdateYerbaMateWithIncorrectIdShouldThrowNotFound()
    {
        var yerbaMateId = Guid.Empty;

        var command = new UpdateYerbaMateCommand
        {
            YerbaMateId = yerbaMateId,
            Name = "Test",
            Description = "Test description",
            AveragePrice = 12.32M,
            CategoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"),
            BrandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239")
        };

        FluentActions.Invoking(() =>
            Mediator.Send(command)).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Update yerba mate command should update yerba mate
    /// </summary>
    [Fact]
    public async Task UpdateYerbaMateShouldUpdateYerbaMate()
    {
        var userId = await AuthHelper.RunAsAdministratorAsync(Factory);

        await TestSeeder.SeedTestBrandsAsync(Factory);
        await TestSeeder.SeedTestCategoriesAsync(Factory);
        await TestSeeder.SeedTestYerbaMatesAsync(Factory);

        var yerbaMateId = Guid.Parse("3C24EB64-6CA5-4716-9A9A-42654F0EAF43"); //one of seeded yerba mates

        var command = new UpdateYerbaMateCommand
        {
            YerbaMateId = yerbaMateId,
            Name = "Updated name",
            Description = "Updated description",
            AveragePrice = 14.32M,
            CategoryId = Guid.Parse("8438FB5B-DC77-40F2-ABB6-C7DCE326571E"), //one of seeded categories
            BrandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239") //one of seeded brands
        };

        await Mediator.Send(command);

        var item = await DbHelper.FindAsync<YerbaMate>(Factory, yerbaMateId);

        item.Name.Should().Be(command.Name);
        item.Description.Should().Be(command.Description);
        item.AveragePrice.Should().Be(command.AveragePrice);
        item.CategoryId.Should().Be(command.CategoryId);
        item.BrandId.Should().Be(command.BrandId);
        item.CreatedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().NotBeNull();
        item.LastModifiedBy.Should().Be(userId);
        item.LastModified.Should().NotBeNull();
        item.LastModified.Should().BeCloseTo(DateTime.Now, 1.Seconds());
    }
}