using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductManager.Application.DTOs.Product;
using ProductManager.Application.Features.Product.Command;
using ProductManager.Application.Features.Product.Query;
using ProductManager.Application.Utilities.Extensions.Identity;
using System.Net;

namespace ProductManager.Api.Controllers;

[Route("api/[controller]/[action]/")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class ProductController : ControllerBase
{
    private readonly IMediator _mediator;
    public ProductController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [AllowAnonymous]
    [HttpPost(Name = "GetProducts")]
    [ProducesResponseType(typeof(FilterProductsDto), (int)HttpStatusCode.OK)]
    public async Task<IActionResult> GetProducts([FromBody] FilterProductsDto filter)
    {
        var result=await _mediator.Send(new GetProductsQuery(filter));
        return Ok(result);
    }

    [HttpPost(Name = "AddProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> AddProduct([FromBody] AddOrEditProductDto addDto)
    {
        await _mediator.Send(new AddProductCommand(addDto, User.GetIdentityUsername()));
        return Ok();
    }

    [HttpPost(Name = "EditProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> EditProduct([FromBody] AddOrEditProductDto editDto)
    {
        await _mediator.Send(new EditProductCommand(editDto, User.GetIdentityUsername()));
        return Ok();
    }

    [HttpDelete( "{productId}", Name = "DeleteProduct")]
    [ProducesResponseType((int)HttpStatusCode.OK)]
    public async Task<IActionResult> DeleteProduct(Guid productId)
    {
        await _mediator.Send(new DeleteProductCommand(productId,User.GetIdentityUsername()));
        return Ok();
    }
}