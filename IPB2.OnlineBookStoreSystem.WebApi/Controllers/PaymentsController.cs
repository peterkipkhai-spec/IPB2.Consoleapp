using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.WebApi.Controllers;

[ApiController]
[Route("api/payments")]
public sealed class PaymentsController(IBookStoreService service) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> MakePayment([FromBody] MakePaymentRequest request, CancellationToken ct)
    {
        await service.MakePaymentAsync(request, ct);
        return NoContent();
    }

    [HttpGet("orders/{orderId:int}")]
    public async Task<IActionResult> GetByOrder(int orderId, CancellationToken ct)
        => Ok(await service.GetPaymentsByOrderAsync(orderId, ct));
}
