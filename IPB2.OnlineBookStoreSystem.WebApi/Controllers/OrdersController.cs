using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.WebApi.Controllers;

[ApiController]
[Route("api/orders")]
public sealed class OrdersController(IBookStoreService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrders(CancellationToken ct) => Ok(await service.GetOrdersAsync(ct));

    [HttpPost]
    public async Task<IActionResult> CreateOrder([FromBody] CreateOrderRequest request, CancellationToken ct)
        => Ok(new { id = await service.CreateOrderAsync(request, ct) });

    [HttpPost("items")]
    public async Task<IActionResult> AddItem([FromBody] AddOrderItemRequest request, CancellationToken ct)
    {
        await service.AddOrderItemAsync(request, ct);
        return NoContent();
    }

    [HttpPut("status")]
    public async Task<IActionResult> UpdateStatus([FromBody] UpdateOrderStatusRequest request, CancellationToken ct)
    {
        await service.UpdateOrderStatusAsync(request, ct);
        return NoContent();
    }

    [HttpPut("{orderId:int}/cancel")]
    public async Task<IActionResult> Cancel(int orderId, CancellationToken ct)
    {
        await service.CancelOrderAsync(orderId, ct);
        return NoContent();
    }
}
