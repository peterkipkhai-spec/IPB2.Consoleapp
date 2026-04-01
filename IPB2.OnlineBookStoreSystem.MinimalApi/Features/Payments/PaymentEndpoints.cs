using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Payments;

public static class PaymentEndpoints
{
    public static IEndpointRouteBuilder MapPaymentEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/payments").WithTags("Payments");
        group.MapPost("/", async (MakePaymentRequest req, IBookStoreService service, CancellationToken ct) => { await service.MakePaymentAsync(req, ct); return Results.NoContent(); });
        group.MapGet("/orders/{orderId:int}", async (int orderId, IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetPaymentsByOrderAsync(orderId, ct)));
        return app;
    }
}
