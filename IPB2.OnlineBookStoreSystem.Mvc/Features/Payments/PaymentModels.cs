namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Payments;

public record PaymentResponse(int PaymentId, int OrderId, DateTime PaymentDate, decimal Amount, string? PaymentMethod);
public record MakePaymentRequest(int OrderId, decimal Amount, string PaymentMethod);
