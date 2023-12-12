using MediatR;
using ProductManager.Application.Services.Interfaces;

namespace ProductManager.Application.Features.Product.Command;

public class DeleteProductCommand : IRequest
{
    public Guid ProductId { get; set; }
    public string UserName { get; }
    public DeleteProductCommand(Guid productId, string userName)
    {
        ProductId = productId;
        UserName = userName;
    }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand>
{
    private readonly IProductService _productService;
    public DeleteProductCommandHandler(IProductService productService)
    {
        _productService = productService;
    }

    public async Task Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        try
        {
            await _productService.DeleteProduct(request.ProductId, request.UserName);
        }
        catch (Exception e)
        {
            throw new Exception(e.Message);
        }
    }
}
