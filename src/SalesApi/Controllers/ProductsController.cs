using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Commands;
using SalesApi.Dtos;
using SalesApi.Queries;

namespace SalesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductsController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await _mediator.Send(new GetProductsQuery());
        return Ok(products);
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateProductCommand request)
    {
        var response = await _mediator.Send(request);
        
        if (response.Status == "success")
            return Ok(response);
        
        return BadRequest(
            new ErrorResponse("InvalidData", response.Message, response.Message));
    }
}