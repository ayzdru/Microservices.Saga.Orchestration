using BuildingBlocks.Core.Exceptions;
using BuildingBlocks.Core.Models;
using BuildingBlocks.EventBus.Messages.Order;
using BuildingBlocks.MassTransit.Interfaces;
using BuildingBlocks.Web.Common;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using Order.Application.Commands.Order.Create;
using Order.Application.Services;
using Order.Core.Enums;
using Order.Infrastructure.Data;
using System.Net.Mime;
using System.Threading;

namespace WebUI.Controllers;

/// <summary>
/// Orders controller
/// </summary>
//[Authorize]
[Produces(MediaTypeNames.Application.Json)]
public class OrdersController : BaseController
{
    private readonly ILogger<OrdersController> _logger;
    private readonly IMassTransitService _massTransitService;
    private readonly OrderDbContext _orderDbContext;
    public OrdersController(ILogger<OrdersController> logger, OrderDbContext applicationDbContext, IMassTransitService massTransitService, ApiGatewayService apiGatewayService, OrderDbContext orderDbContext)
    {
        _logger = logger;
        _massTransitService = massTransitService;
        _orderDbContext = orderDbContext;
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
        var result = await Mediator.Send(command);
        // MassTransit Outbox ile eventleri gönderebilmek için doğrudan dbContext.SaveChanges() çağrısı gereklidir.
        // Bu sebeple IApplicationDbContext arayüzü kullanılamaz; çünkü Outbox mekanizması DbContext'in kendisine ihtiyaç duyar.
        // Gerekli nesneler Application katmanından alınır, transaction başlatılır ve SaveChangesAsync metodu kullanılır.
        //
        // To send events using MassTransit Outbox, you must call dbContext.SaveChanges() directly.
        // Therefore, the IApplicationDbContext interface cannot be used, as the Outbox mechanism requires the actual DbContext instance.
        // Required objects are retrieved from the Application layer, a transaction is started, and the SaveChangesAsync method is used.
        await _orderDbContext.SaveChangesAsync();
        return result;
    }
}