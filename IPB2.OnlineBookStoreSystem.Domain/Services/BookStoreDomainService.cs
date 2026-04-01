using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Entities;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;

namespace IPB2.OnlineBookStoreSystem.Domain.Services;

public sealed class BookStoreDomainService(IBookStoreRepository repository) : IBookStoreService
{
    public Task<IReadOnlyList<Author>> GetAuthorsAsync(CancellationToken ct = default) => repository.GetAuthorsAsync(ct);
    public Task<int> CreateAuthorAsync(UpsertAuthorRequest request, CancellationToken ct = default) => repository.CreateAuthorAsync(request, ct);
    public Task UpdateAuthorAsync(int id, UpsertAuthorRequest request, CancellationToken ct = default) => repository.UpdateAuthorAsync(id, request, ct);
    public Task DeleteAuthorAsync(int id, CancellationToken ct = default) => repository.DeleteAuthorAsync(id, ct);

    public Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken ct = default) => repository.GetCategoriesAsync(ct);
    public Task<int> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken ct = default) => repository.CreateCategoryAsync(request, ct);
    public Task UpdateCategoryAsync(int id, UpsertCategoryRequest request, CancellationToken ct = default) => repository.UpdateCategoryAsync(id, request, ct);
    public Task DeleteCategoryAsync(int id, CancellationToken ct = default) => repository.DeleteCategoryAsync(id, ct);

    public Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default) => repository.GetCustomersAsync(ct);
    public Task<int> CreateCustomerAsync(UpsertCustomerRequest request, CancellationToken ct = default) => repository.CreateCustomerAsync(request, ct);
    public Task UpdateCustomerAsync(int id, UpsertCustomerRequest request, CancellationToken ct = default) => repository.UpdateCustomerAsync(id, request, ct);
    public Task DeleteCustomerAsync(int id, CancellationToken ct = default) => repository.DeleteCustomerAsync(id, ct);

    public Task<IReadOnlyList<Book>> GetBooksAsync(CancellationToken ct = default) => repository.GetBooksAsync(ct);
    public Task<int> CreateBookAsync(UpsertBookRequest request, CancellationToken ct = default) => repository.CreateBookAsync(request, ct);
    public Task UpdateBookAsync(int id, UpsertBookRequest request, CancellationToken ct = default) => repository.UpdateBookAsync(id, request, ct);
    public Task DeleteBookAsync(int id, CancellationToken ct = default) => repository.DeleteBookAsync(id, ct);

    public Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken ct = default) => repository.GetOrdersAsync(ct);
    public Task<IReadOnlyList<OrderItem>> GetOrderItemsAsync(int orderId, CancellationToken ct = default) => repository.GetOrderItemsAsync(orderId, ct);
    public Task<int> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default) => repository.CreateOrderAsync(request, ct);
    public Task AddOrderItemAsync(AddOrderItemRequest request, CancellationToken ct = default) => repository.AddOrderItemAsync(request, ct);
    public Task UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default) => repository.UpdateOrderStatusAsync(request, ct);
    public Task CancelOrderAsync(int orderId, CancellationToken ct = default) => repository.CancelOrderAsync(orderId, ct);

    public Task MakePaymentAsync(MakePaymentRequest request, CancellationToken ct = default) => repository.MakePaymentAsync(request, ct);
    public Task<IReadOnlyList<Payment>> GetPaymentsByOrderAsync(int orderId, CancellationToken ct = default) => repository.GetPaymentsByOrderAsync(orderId, ct);

    public Task<IReadOnlyList<SalesReportRow>> GetSalesReportAsync(CancellationToken ct = default) => repository.GetSalesReportAsync(ct);
    public Task<IReadOnlyList<StockReportRow>> GetStockReportAsync(CancellationToken ct = default) => repository.GetStockReportAsync(ct);
    public Task<IReadOnlyList<CustomerOrderSummaryRow>> GetCustomerOrderSummaryAsync(CancellationToken ct = default) => repository.GetCustomerOrderSummaryAsync(ct);
}
