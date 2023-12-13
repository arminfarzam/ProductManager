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
    public async Task GetProducts_ShouldReturnFilteredProducts()
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
    public async Task TestGetProductsQueryService()
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
        var result= await productService.GetProducts(filter);
        //assert
        Assert.NotNull(result);
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