using IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Payments;

public sealed class PaymentsController(BookStoreApiClient api) : Controller
{
    public async Task<IActionResult> Index(int? orderId, CancellationToken ct)
    {
        if (orderId is null) return View(Array.Empty<PaymentResponse>());
        return View(await api.GetListAsync<PaymentResponse>($"/api/payments/orders/{orderId}", ct));
    }

    [HttpPost]
    public async Task<IActionResult> Create(MakePaymentRequest request, CancellationToken ct)
    {
        await api.PostAsync("/api/payments", request, ct);
        return RedirectToAction(nameof(Index), new { orderId = request.OrderId });
    }
}
