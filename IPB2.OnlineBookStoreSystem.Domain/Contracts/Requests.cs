namespace IPB2.OnlineBookStoreSystem.Domain.Contracts;

public record UpsertAuthorRequest(string AuthorName, string? Bio);
public record UpsertCategoryRequest(string CategoryName, string? Description);
public record UpsertCustomerRequest(string FullName, string? Email, string? Phone, string? Address);
public record UpsertBookRequest(string Title, int AuthorId, int CategoryId, decimal Price, int Stock, int? PublishedYear, string? Isbn);
public record CreateOrderRequest(int CustomerId);
public record AddOrderItemRequest(int OrderId, int BookId, int Quantity);
public record UpdateOrderStatusRequest(int OrderId, string Status);
public record MakePaymentRequest(int OrderId, decimal Amount, string PaymentMethod);
