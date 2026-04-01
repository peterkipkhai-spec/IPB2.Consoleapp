using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Categories;

public sealed class CategoriesController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await api.GetListAsync<CategoryResponse>("/api/categories", ct));
    [HttpPost] public async Task<IActionResult> Create(UpsertCategoryRequest request, CancellationToken ct){ await api.PostAsync("/api/categories", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> Delete(int id, CancellationToken ct){ await api.DeleteAsync($"/api/categories/{id}", ct); return RedirectToAction(nameof(Index)); }
}
