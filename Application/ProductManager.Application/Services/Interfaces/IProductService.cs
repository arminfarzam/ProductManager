using ProductManager.Application.DTOs.Product;

namespace ProductManager.Application.Services.Interfaces;

public interface IProductService
{
    Task<FilterProductsDto> GetProducts(FilterProductsDto filter);
    Task AddProduct(AddOrEditProductDto addDto,string userName);
    Task EditProduct(AddOrEditProductDto editDto,string userName);
    Task DeleteProduct(Guid productId,string userName);
}