using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Customers;

public static class CustomerEndpoints
{
    public static IEndpointRouteBuilder MapCustomerEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/customers").WithTags("Customers");
        group.MapGet("/", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetCustomersAsync(ct)));
        group.MapPost("/", async (UpsertCustomerRequest req, IBookStoreService service, CancellationToken ct) => Results.Ok(new { id = await service.CreateCustomerAsync(req, ct) }));
        group.MapPut("/{id:int}", async (int id, UpsertCustomerRequest req, IBookStoreService service, CancellationToken ct) => { await service.UpdateCustomerAsync(id, req, ct); return Results.NoContent(); });
        group.MapDelete("/{id:int}", async (int id, IBookStoreService service, CancellationToken ct) => { await service.DeleteCustomerAsync(id, ct); return Results.NoContent(); });
        return app;
    }
}
