using System.Net.Mime;
using BuildingBlocks.Core.Models;
using BuildingBlocks.Web.Common;
using Microsoft.AspNetCore.Mvc;
using Order.Application.Commands.Order.Create;

namespace WebUI.Controllers;

/// <summary>
/// Orders controller
/// </summary>
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController : BaseController
{
    private readonly ILogger<OrdersController> _logger;    

    public OrdersController(ILogger<OrdersController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Creates an Order
    /// </summary> 
    /// <returns></returns>
    [HttpPost, Route("api/orders")]
    [Consumes(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResult<string>), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResult<string>>> CreateOrder([FromBody] CreateOrderCommand command)
    {          
        return await Mediator.Send(command);
    }
}