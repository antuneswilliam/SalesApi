using MediatR;
using Microsoft.AspNetCore.Mvc;
using SalesApi.Commands;
using SalesApi.Dtos;
using SalesApi.Queries;

namespace SalesApi.Controllers;

[ApiController]
[Route("[controller]")]
public class SalesController : ControllerBase
{
    private readonly IMediator _mediator;

    public SalesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var sales = await _mediator.Send(new GetSalesQuery());
        return Ok(sales);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateSaleCommand request)
    {
        var response = await _mediator.Send(request);

        if (response.Status == "success")
            return Ok(response);

        return BadRequest(
            new ErrorResponse("InvalidData", "Invalid Sell", response.Message));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var response = await _mediator.Send(new CancelSaleCommand(id));

        if (response.Status == "success")
            return Ok(response);

        return NotFound(
            new ErrorResponse("NotFound", response.Message, response.Message));
    }
}