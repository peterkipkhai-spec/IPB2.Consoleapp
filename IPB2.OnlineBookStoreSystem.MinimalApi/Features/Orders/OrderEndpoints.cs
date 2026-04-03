using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Orders;

public static class OrderEndpoints
{
    public static IEndpointRouteBuilder MapOrderEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");
        group.MapGet("/", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetOrdersAsync(ct)));
        group.MapGet("/{id:int}/items", async (int id, IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetOrderItemsAsync(id, ct)));
        group.MapPost("/", async (CreateOrderRequest req, IBookStoreService service, CancellationToken ct) => Results.Ok(new { id = await service.CreateOrderAsync(req, ct) }));
        group.MapPost("/items", async (AddOrderItemRequest req, IBookStoreService service, CancellationToken ct) => { await service.AddOrderItemAsync(req, ct); return Results.NoContent(); });
        group.MapPut("/status", async (UpdateOrderStatusRequest req, IBookStoreService service, CancellationToken ct) => { await service.UpdateOrderStatusAsync(req, ct); return Results.NoContent(); });
        group.MapPut("/{id:int}/cancel", async (int id, IBookStoreService service, CancellationToken ct) => { await service.CancelOrderAsync(id, ct); return Results.NoContent(); });
        return app;
    }
}
