using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Authors;

public sealed class AuthorsController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct)
        => View(await api.GetListAsync<AuthorResponse>("/api/authors", ct));

    [HttpPost]
    public async Task<IActionResult> Create(UpsertAuthorRequest request, CancellationToken ct)
    {
        await api.PostAsync("/api/authors", request, ct);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken ct)
    {
        await api.DeleteAsync($"/api/authors/{id}", ct);
        return RedirectToAction(nameof(Index));
    }
}
