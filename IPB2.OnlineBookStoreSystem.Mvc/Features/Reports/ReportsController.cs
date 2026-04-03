using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Reports;

public sealed class ReportsController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var sales = await api.GetListAsync<SalesReportResponse>("/api/reports/sales", ct);
        var stock = await api.GetListAsync<StockReportResponse>("/api/reports/stock", ct);
        var customer = await api.GetListAsync<CustomerOrderSummaryResponse>("/api/reports/customer-orders", ct);
        return View(new ReportsVm(sales, stock, customer));
    }
}
