using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.WebApi.Controllers;

[ApiController]
[Route("api/reports")]
public sealed class ReportsController(IBookStoreService service) : ControllerBase
{
    [HttpGet("sales")]
    public async Task<IActionResult> Sales(CancellationToken ct) => Ok(await service.GetSalesReportAsync(ct));

    [HttpGet("stock")]
    public async Task<IActionResult> Stock(CancellationToken ct) => Ok(await service.GetStockReportAsync(ct));

    [HttpGet("customer-orders")]
    public async Task<IActionResult> CustomerOrders(CancellationToken ct) => Ok(await service.GetCustomerOrderSummaryAsync(ct));
}
