using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Reports;

public static class ReportEndpoints
{
    public static IEndpointRouteBuilder MapReportEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/reports").WithTags("Reports");
        group.MapGet("/sales", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetSalesReportAsync(ct)));
        group.MapGet("/stock", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetStockReportAsync(ct)));
        group.MapGet("/customer-orders", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetCustomerOrderSummaryAsync(ct)));
        return app;
    }
}
