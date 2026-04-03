namespace IPB2.OnlineBookStoreSystem.Domain.Entities;

public record Author(int AuthorId, string AuthorName, string? Bio, DateTime CreatedAt);
public record Category(int CategoryId, string CategoryName, string? Description, DateTime CreatedAt);
public record Customer(int CustomerId, string FullName, string? Email, string? Phone, string? Address, DateTime CreatedAt);
public record Book(int BookId, string Title, int AuthorId, string AuthorName, int CategoryId, string CategoryName, decimal Price, int Stock, int? PublishedYear, string? Isbn, DateTime CreatedAt);
public record Order(int OrderId, int CustomerId, string FullName, DateTime OrderDate, decimal TotalAmount, string Status);
public record OrderItem(int OrderDetailId, int OrderId, int BookId, string Title, int Quantity, decimal UnitPrice, decimal SubTotal);
public record Payment(int PaymentId, int OrderId, DateTime PaymentDate, decimal Amount, string? PaymentMethod);
public record SalesReportRow(DateTime Date, decimal TotalSales, int PaymentCount);
public record StockReportRow(int BookId, string Title, int Stock, decimal Price, string AuthorName, string CategoryName);
public record CustomerOrderSummaryRow(int CustomerId, string FullName, int TotalOrders, decimal TotalSpent);
