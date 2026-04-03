using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Books;

public static class BookEndpoints
{
    public static IEndpointRouteBuilder MapBookEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/books").WithTags("Books");
        group.MapGet("/", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetBooksAsync(ct)));
        group.MapPost("/", async (UpsertBookRequest req, IBookStoreService service, CancellationToken ct) => Results.Ok(new { id = await service.CreateBookAsync(req, ct) }));
        group.MapPut("/{id:int}", async (int id, UpsertBookRequest req, IBookStoreService service, CancellationToken ct) => { await service.UpdateBookAsync(id, req, ct); return Results.NoContent(); });
        group.MapDelete("/{id:int}", async (int id, IBookStoreService service, CancellationToken ct) => { await service.DeleteBookAsync(id, ct); return Results.NoContent(); });
        return app;
    }
}
