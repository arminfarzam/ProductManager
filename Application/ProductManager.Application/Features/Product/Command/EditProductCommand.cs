using MediatR;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Product.Command;

public class EditProductCommand : IRequest
{
    public AddOrEditProductDto Dto { get; }
    public string UserName { get; }
    public EditProductCommand(AddOrEditProductDto dto, string userName)
    {
        Dto = dto;
        UserName = userName;
    }
}

public class EditProductCommandHandler : IRequestHandler<EditProductCommand>
{
    private readonly IProductService _productService;
    public EditProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.EditProduct(request.Dto, request.UserName);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}