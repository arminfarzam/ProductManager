using ProductManager.Application.DTOs.Paging;

namespace ProductManager.Application.DTOs.Product;

public class FilterProductsDto : BasePaging
{
    public string? SearchQuery { get; set; }
    public List<ProductDto>? Products { get; set; }
    public FilterProductsDto SetPaging(BasePaging paging)
    {
        PageNumber = paging.PageNumber;
        PageCount = paging.PageCount;
        PageSize = paging.PageSize;
        SkipSize = paging.SkipSize;
        return this;
    }
    public FilterProductsDto SetProducts(List<ProductDto> products)
    {
        Products = products;
        return this;
    }
}