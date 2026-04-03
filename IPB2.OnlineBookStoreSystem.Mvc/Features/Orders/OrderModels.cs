namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Orders;

public record OrderResponse(int OrderId, int CustomerId, string FullName, DateTime OrderDate, decimal TotalAmount, string Status);
public record CreateOrderRequest(int CustomerId);
public record AddOrderItemRequest(int OrderId, int BookId, int Quantity);
public record UpdateOrderStatusRequest(int OrderId, string Status);
public record OrderItemResponse(int OrderDetailId, int OrderId, int BookId, string Title, int Quantity, decimal UnitPrice, decimal SubTotal);
