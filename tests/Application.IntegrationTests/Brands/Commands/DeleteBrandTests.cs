﻿using System;
using System.Threading.Tasks;
using Application.Brands.Commands.DeleteBrand;
using Application.Common.Exceptions;
using Application.IntegrationTests.Helpers;
using Domain.Entities;
using FluentAssertions;
using Xunit;

namespace Application.IntegrationTests.Brands.Commands;

/// <summary>
///     Delete brand tests
/// </summary>
public class DeleteBrandTests : IntegrationTest
{
    /// <summary>
    ///     Delete brand with incorrect id should throw not found exception
    /// </summary>
    [Fact]
    public void DeleteBrandWithIncorrectIdShouldThrowNotFound()
    {
        var brandId = Guid.Empty;

        FluentActions.Invoking(() =>
            Mediator.Send(new DeleteBrandCommand {BrandId = brandId})).Should().ThrowAsync<NotFoundException>();
    }

    /// <summary>
    ///     Delete brand command should delete brand
    /// </summary>
    [Fact]
    public async Task ShouldDeleteBrand()
    {
        await TestSeeder.SeedTestBrandsAsync(Factory);

        var brandId = Guid.Parse("17458BDE-3849-4150-B73A-A492A8F7F239"); //one of seeded brand

        //delete
        await Mediator.Send(new DeleteBrandCommand {BrandId = brandId});

        //Assert that deleted
        var item = await DbHelper.FindAsync<Brand>(Factory, brandId);
        item.Should().BeNull();
    }
}