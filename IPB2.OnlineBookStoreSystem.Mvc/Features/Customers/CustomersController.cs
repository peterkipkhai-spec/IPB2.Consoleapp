using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Customers;

public sealed class CustomersController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await api.GetListAsync<CustomerResponse>("/api/customers", ct));
    [HttpPost] public async Task<IActionResult> Create(UpsertCustomerRequest request, CancellationToken ct){ await api.PostAsync("/api/customers", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> Delete(int id, CancellationToken ct){ await api.DeleteAsync($"/api/customers/{id}", ct); return RedirectToAction(nameof(Index)); }
}
