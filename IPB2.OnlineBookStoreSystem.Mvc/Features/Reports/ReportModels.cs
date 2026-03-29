namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Reports;

public record SalesReportResponse(DateTime Date, decimal TotalSales, int PaymentCount);
public record StockReportResponse(int BookId, string Title, int Stock, decimal Price, string AuthorName, string CategoryName);
public record CustomerOrderSummaryResponse(int CustomerId, string FullName, int TotalOrders, decimal TotalSpent);
public record ReportsVm(IReadOnlyList<SalesReportResponse> Sales, IReadOnlyList<StockReportResponse> Stock, IReadOnlyList<CustomerOrderSummaryResponse> CustomerSummary);
