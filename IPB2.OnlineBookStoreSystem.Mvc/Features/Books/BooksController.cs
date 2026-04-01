using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Books;

public sealed class BooksController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await api.GetListAsync<BookResponse>("/api/books", ct));
    [HttpPost] public async Task<IActionResult> Create(UpsertBookRequest request, CancellationToken ct){ await api.PostAsync("/api/books", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> Delete(int id, CancellationToken ct){ await api.DeleteAsync($"/api/books/{id}", ct); return RedirectToAction(nameof(Index)); }
}
