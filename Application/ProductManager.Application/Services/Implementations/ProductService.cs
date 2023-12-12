using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductManager.Application.DTOs.Paging;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Services.Interfaces;
using ProductManager.Application.Utilities.Paging;
using ProductManager.Domain.Entities.Product;
using ProductManager.Domain.Repositories.Common;

namespace ProductManager.Application.Services.Implementations;

public class ProductService:IProductService
{
    private readonly IGenericRepository<Product> _productRepository;
    private readonly IMapper _mapper;
    public ProductService(IGenericRepository<Product> productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<FilterProductsDto> GetProducts(FilterProductsDto filter)
    {
        var productsQuery = _productRepository.GetEntitiesQueryable().Where(p => !p.IsDeleted);
        //Filter By Product Name Or Creator UserName
        if (!string.IsNullOrEmpty(filter.SearchQuery))
            productsQuery = productsQuery.Where(p => p.Name.Contains(filter.SearchQuery)|| p.CreatorUserName.Contains(filter.SearchQuery));

        //Handle Pagination
        var pageCount = (int)Math.Ceiling(productsQuery.Count() / (double)filter.PageSize);
        var pager = Pager.Build(filter.PageNumber, filter.PageSize, pageCount);
        var products = await productsQuery.Paging(pager).Select(p => _mapper.Map<ProductDto>(p)).ToListAsync();
        return filter.SetProducts(products).SetPaging(pager);
    }

    public async Task AddProduct(AddOrEditProductDto addDto, string userName)
    {
        var product = _mapper.Map<Product>(addDto);
        product.CreatorUserName = userName;
        await _productRepository.AddEntity(product);
        await _productRepository.SaveChanges();
    }

    public async Task EditProduct(AddOrEditProductDto editDto, string userName)
    {
        var product = await GetProductById(editDto.Id);
        if (product.CreatorUserName!=userName)
            throw new Exception("You Don't Have Access To Edit This Product");
        product = _mapper.Map(editDto, product);
        _productRepository.UpdateEntity(product);
        await _productRepository.SaveChanges();
    }

    public async Task DeleteProduct(Guid productId, string userName)
    {
        var product = await GetProductById(productId);
        if (product.CreatorUserName != userName)
            throw new Exception("You Don't Have Access To Delete This Product");
        _productRepository.RemoveEntity(product);
        await _productRepository.SaveChanges();
    }

    #region Private

    private async Task<Product> GetProductById(Guid id)
    {
        var product = await _productRepository.GetEntityById(id);
        if (product == null)
            throw new Exception("Product Not Found");
        return product;
    }

    #endregion
}