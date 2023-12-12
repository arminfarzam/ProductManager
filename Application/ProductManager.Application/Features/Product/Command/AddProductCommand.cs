using MediatR;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Product.Command;

public class AddProductCommand:IRequest
{
    public AddOrEditProductDto Dto { get; }
    public string UserName { get; }
    public AddProductCommand(AddOrEditProductDto dto, string userName)
    {
        Dto = dto;
        UserName = userName;
    }
}

public class AddProductCommandHandler : IRequestHandler<AddProductCommand>
{
    private readonly IProductService _productService;
    public AddProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.AddProduct(request.Dto, request.UserName);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}