using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Authors;

public static class AuthorEndpoints
{
    public static IEndpointRouteBuilder MapAuthorEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/authors").WithTags("Authors");
        group.MapGet("/", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetAuthorsAsync(ct)));
        group.MapPost("/", async (UpsertAuthorRequest req, IBookStoreService service, CancellationToken ct) => Results.Ok(new { id = await service.CreateAuthorAsync(req, ct) }));
        group.MapPut("/{id:int}", async (int id, UpsertAuthorRequest req, IBookStoreService service, CancellationToken ct) => { await service.UpdateAuthorAsync(id, req, ct); return Results.NoContent(); });
        group.MapDelete("/{id:int}", async (int id, IBookStoreService service, CancellationToken ct) => { await service.DeleteAuthorAsync(id, ct); return Results.NoContent(); });
        return app;
    }
}
