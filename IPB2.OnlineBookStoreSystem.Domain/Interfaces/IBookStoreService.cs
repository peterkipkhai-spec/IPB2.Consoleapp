using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Entities;

namespace IPB2.OnlineBookStoreSystem.Domain.Interfaces;

public interface IBookStoreService
{
    Task<IReadOnlyList<Author>> GetAuthorsAsync(CancellationToken ct = default);
    Task<int> CreateAuthorAsync(UpsertAuthorRequest request, CancellationToken ct = default);
    Task UpdateAuthorAsync(int id, UpsertAuthorRequest request, CancellationToken ct = default);
    Task DeleteAuthorAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken ct = default);
    Task<int> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken ct = default);
    Task UpdateCategoryAsync(int id, UpsertCategoryRequest request, CancellationToken ct = default);
    Task DeleteCategoryAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default);
    Task<int> CreateCustomerAsync(UpsertCustomerRequest request, CancellationToken ct = default);
    Task UpdateCustomerAsync(int id, UpsertCustomerRequest request, CancellationToken ct = default);
    Task DeleteCustomerAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Book>> GetBooksAsync(CancellationToken ct = default);
    Task<int> CreateBookAsync(UpsertBookRequest request, CancellationToken ct = default);
    Task UpdateBookAsync(int id, UpsertBookRequest request, CancellationToken ct = default);
    Task DeleteBookAsync(int id, CancellationToken ct = default);

    Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken ct = default);
    Task<IReadOnlyList<OrderItem>> GetOrderItemsAsync(int orderId, CancellationToken ct = default);
    Task<int> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default);
    Task AddOrderItemAsync(AddOrderItemRequest request, CancellationToken ct = default);
    Task UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default);
    Task CancelOrderAsync(int orderId, CancellationToken ct = default);

    Task MakePaymentAsync(MakePaymentRequest request, CancellationToken ct = default);
    Task<IReadOnlyList<Payment>> GetPaymentsByOrderAsync(int orderId, CancellationToken ct = default);

    Task<IReadOnlyList<SalesReportRow>> GetSalesReportAsync(CancellationToken ct = default);
    Task<IReadOnlyList<StockReportRow>> GetStockReportAsync(CancellationToken ct = default);
    Task<IReadOnlyList<CustomerOrderSummaryRow>> GetCustomerOrderSummaryAsync(CancellationToken ct = default);
}
