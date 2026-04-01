using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Features.Categories;

public static class CategoryEndpoints
{
    public static IEndpointRouteBuilder MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categories");
        group.MapGet("/", async (IBookStoreService service, CancellationToken ct) => Results.Ok(await service.GetCategoriesAsync(ct)));
        group.MapPost("/", async (UpsertCategoryRequest req, IBookStoreService service, CancellationToken ct) => Results.Ok(new { id = await service.CreateCategoryAsync(req, ct) }));
        group.MapPut("/{id:int}", async (int id, UpsertCategoryRequest req, IBookStoreService service, CancellationToken ct) => { await service.UpdateCategoryAsync(id, req, ct); return Results.NoContent(); });
        group.MapDelete("/{id:int}", async (int id, IBookStoreService service, CancellationToken ct) => { await service.DeleteCategoryAsync(id, ct); return Results.NoContent(); });
        return app;
    }
}
