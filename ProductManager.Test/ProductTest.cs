using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Features.Product.Query;
using ProductManager.Application.Mapping;
using ProductManager.Application.Services.Implementations;
using ProductManager.Application.Services.Interfaces;
using ProductManager.Data.Context;
using ProductManager.Data.Repositories.Common;
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
        var addProductDto = new AddOrEditProductDto()
        {
            Name = "Galaxy S23",
            ManufacturerEmail = "sam4@gmail.com",
            ManufacturerPhone = "09216443851",
            IsAvailable = false
        };
        await productService.AddProduct(addProductDto, "arminfrzm72");

        // Act
        var result = await productService.GetProducts(filter);

        //assert
        result.ShouldNotBeNull();
        result.Products!.Count.ShouldBeGreaterThan(0);
    }


    [Fact]
    public async Task TestGetProductsQueryHandler()
    {
        //arrange
        var serviceProvider = GetServiceProvider();
        var productService = serviceProvider.GetRequiredService<IProductService>();
        var filter = new FilterProductsDto()
        {
            PageNumber = 1,
            PageSize = 20
        };
        var addProductDto = new AddOrEditProductDto()
        {
            Name = "Galaxy S23",
            ManufacturerEmail = "sam4@gmail.com",
            ManufacturerPhone = "09216443851",
            IsAvailable = false
        };
        await productService.AddProduct(addProductDto, "arminfrzm72");

        // Act
        var handler = new GetProductsQueryHandler(productService);
        var result = await handler.Handle(new GetProductsQuery(filter), CancellationToken.None);

        //assert
        Assert.NotNull(result);
        result.Products!.Count.ShouldBeGreaterThan(0);
    }


    [Fact]
    public async Task TestAddProduct()
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
    public async Task TestEditProduct()
    {
        // Arrange
        var productService = new ProductService(_mockRepository.Object, _mapper);
        var editProductDto = new AddOrEditProductDto()
        {
            Id = Guid.Parse("e4858aff-77e3-477e-2d94-08dbfb53603b"),
            Name = "Galaxy A8",
            ManufacturerEmail = "samsung@gmail.com",
            ManufacturerPhone = "09215483856",
            IsAvailable = true,
        };

        // Act
        await productService.EditProduct(editProductDto, "arminfrzm72");
        var editedProduct = _mockRepository.Object.GetEntitiesQueryable().FirstOrDefault(p => p.Id == Guid.Parse("e4858aff-77e3-477e-2d94-08dbfb53603b"));

        // Assert
        editedProduct!.Name.ShouldBe("Galaxy A8");
    }


    [Fact]
    public async Task TestRemoveProduct()
    {
        // Arrange
        var productService = new ProductService(_mockRepository.Object, _mapper);
        var productIdToRemove = Guid.Parse("e4858aff-77e3-477e-2d94-08dbfb53603b");

        // Act
        await productService.DeleteProduct(productIdToRemove, "arminfrzm72");

        // Assert
        var productsAfterRemoval = _mockRepository.Object.GetEntitiesQueryable().ToList();
        Assert.DoesNotContain(productsAfterRemoval, p => p.Id == productIdToRemove);
    }

    #region Handle Services

    private IServiceProvider GetServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddScoped<IProductService, ProductService>();
        var dbContextOptions = new DbContextOptionsBuilder<ProductManagerContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
        services.AddSingleton(new ProductManagerContext(dbContextOptions));
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        return services.BuildServiceProvider();
    }

    #endregion
}