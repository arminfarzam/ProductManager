using MediatR;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Product.Query;

public class GetProductsQuery:IRequest<FilterProductsDto>
{
    public FilterProductsDto Dto { get; }
    public GetProductsQuery(FilterProductsDto dto)
    {
        Dto = dto;
    }
}

public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, FilterProductsDto>
{
    private readonly IProductService _productService;
    public GetProductsQueryHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task<FilterProductsDto> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var result = await _productService.GetProducts(request.Dto);
            return result;
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}