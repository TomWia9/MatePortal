using System;
using System.Runtime.Serialization;
using Application.Brands.Queries;
using Application.Categories.Queries;
using Application.Common.Mappings;
using Application.Countries.Queries;
using Application.Favourites.Queries;
using Application.Opinions.Queries;
using Application.ShopOpinions.Queries;
using Application.Shops.Queries;
using Application.YerbaMates.Queries;
using AutoMapper;
using Domain.Entities;
using Xunit;

namespace Application.UnitTests.Common.Mappings
{
    public class MappingTests
    {
        private readonly IConfigurationProvider _configuration;
        private readonly IMapper _mapper;

        public MappingTests()
        {
            _configuration = new MapperConfiguration(cfg => { cfg.AddProfile<MappingProfile>(); });

            _mapper = _configuration.CreateMapper();
        }

        [Fact]
        public void ShouldHaveValidConfiguration()
        {
            _configuration.AssertConfigurationIsValid();
        }

        [Theory]
        [InlineData(typeof(Brand), typeof(BrandDto))]
        [InlineData(typeof(Category), typeof(CategoryDto))]
        [InlineData(typeof(Country), typeof(CountryDto))]
        [InlineData(typeof(Favourite), typeof(FavouriteDto))]
        [InlineData(typeof(Opinion), typeof(OpinionDto))]
        [InlineData(typeof(ShopOpinion), typeof(ShopOpinionDto))]
        [InlineData(typeof(Shop), typeof(ShopDto))]
        [InlineData(typeof(YerbaMate), typeof(YerbaMateDto))]
        //[InlineData(typeof(ApplicationUser), typeof(UserDto))]
        public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
        {
            var instance = GetInstanceOf(source);
            _mapper.Map(instance, source, destination);
        }

        private static object GetInstanceOf(Type type)
        {
            if (type.GetConstructor(Type.EmptyTypes) != null)
                return Activator.CreateInstance(type);

            // Type without parameterless constructor
            return FormatterServices.GetUninitializedObject(type);
        }
    }
}