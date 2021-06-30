using System;
using System.Runtime.Serialization;
using Application.Brands.Queries.GetBrands;
using Application.Categories.Queries.GetCategories;
using Application.Common.Mappings;
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
            _configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

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