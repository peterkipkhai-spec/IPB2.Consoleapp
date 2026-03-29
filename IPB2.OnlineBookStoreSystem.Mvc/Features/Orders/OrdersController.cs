using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Orders;

public sealed class OrdersController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(CancellationToken ct) => View(await api.GetListAsync<OrderResponse>("/api/orders", ct));
    [HttpPost] public async Task<IActionResult> Create(CreateOrderRequest request, CancellationToken ct){ await api.PostAsync("/api/orders", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> AddItem(AddOrderItemRequest request, CancellationToken ct){ await api.PostAsync("/api/orders/items", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> UpdateStatus(UpdateOrderStatusRequest request, CancellationToken ct){ await api.PutAsync("/api/orders/status", request, ct); return RedirectToAction(nameof(Index)); }
    [HttpPost] public async Task<IActionResult> Cancel(int id, CancellationToken ct){ await api.PutAsync($"/api/orders/{id}/cancel", ct); return RedirectToAction(nameof(Index)); }
}
