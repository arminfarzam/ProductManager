using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Features.Product.Query;
using ProductManager.Application.Mapping;
using ProductManager.Application.Services.Implementations;
using ProductManager.Application.Services.Interfaces;
using ProductManager.Domain.Entities.Product;
using ProductManager.Domain.Repositories.Common;
using ProductManager.Test.Mocks;
using Shouldly;

namespace ProductManager.Test;

public class ProductTest
{
    private readonly Mock<IGenericRepository<Product>> _mockRepository;
    private readonly IMapper _mapper;
    public ProductTest()
    {
        _mockRepository = MockProductRepository.GetProductRepository();
        var mapConfig = new MapperConfiguration(m =>
        {
            m.AddProfile<MappingProfile>();
        });
        _mapper = mapConfig.CreateMapper();
    }

    [Fact]
    public async Task GetProductsShouldReturnFilteredProducts()
    {
        // Arrange
        var filter = new FilterProductsDto()
        {
            PageNumber = 1,
            PageSize = 20
        };

        var productService = new ProductService(_mockRepository.Object, _mapper);

        // Act
        var result = await productService.GetProducts(filter);

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task AddProduct()
    {
        // Arrange
        var initialProductCount = 2;
        var productService = new ProductService(_mockRepository.Object, _mapper);
        var addProductDto = new AddOrEditProductDto()
        {
            Name = "Galaxy S23",
            ManufacturerEmail = "sam4@gmail.com",
            ManufacturerPhone = "09216443851",
            IsAvailable = false
        };

        // Act
        await productService.AddProduct(addProductDto, "arminfrzm72");

        // Assert
        Assert.Equal(initialProductCount + 1, _mockRepository.Object.GetEntitiesQueryable().Count());
    }

    [Fact]
    public async Task TestGetProductsService()
    {
        //arrange
        var serviceProvider = GetServiceProvider();
        var productService = serviceProvider.GetRequiredService<IProductService>();
        var filter = new FilterProductsDto()
        {
            PageNumber = 1,
            PageSize = 20
        };

        //act
        var result = await productService.GetProducts(filter);

        //assert
        result.ShouldNotBeNull();
    }

    [Fact]
    public async Task TestGetProductsQueryHandler()
    {
        var serviceProvider = GetServiceProvider();
        var productService = serviceProvider.GetRequiredService<IProductService>();
        var filter = new FilterProductsDto()
        {
            PageNumber = 1,
            PageSize = 20
        };
        var handler = new GetProductsQueryHandler(productService);
        var result = await handler.Handle(new GetProductsQuery(filter), CancellationToken.None);
        Assert.NotNull(result);
    }

    private IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddScoped<IProductService, ProductService>();
        return services.BuildServiceProvider();
    }
}