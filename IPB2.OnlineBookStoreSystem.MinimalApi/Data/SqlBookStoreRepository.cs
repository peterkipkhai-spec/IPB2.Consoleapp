using System.Data;
using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Entities;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using Microsoft.Data.SqlClient;

namespace IPB2.OnlineBookStoreSystem.MinimalApi.Data;

public sealed class SqlBookStoreRepository(IConfiguration configuration) : IBookStoreRepository
{
    private readonly string _connectionString = configuration.GetConnectionString("OnlineBookStoreDb")
        ?? throw new InvalidOperationException("Connection string 'OnlineBookStoreDb' is missing.");

    private SqlConnection Connection() => new(_connectionString);

    public Task<IReadOnlyList<Author>> GetAuthorsAsync(CancellationToken ct = default) =>
        QueryAsync("SELECT AuthorId, AuthorName, Bio, CreatedAt FROM Authors ORDER BY AuthorId DESC",
            r => new Author(r.GetInt32(0), r.GetString(1), r.IsDBNull(2) ? null : r.GetString(2), r.GetDateTime(3)), ct);

    public async Task<int> CreateAuthorAsync(UpsertAuthorRequest request, CancellationToken ct = default)
    {
        const string sql = "INSERT INTO Authors (AuthorName, Bio) VALUES (@name,@bio); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", request.AuthorName);
        cmd.Parameters.AddWithValue("@bio", (object?)request.Bio ?? DBNull.Value);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
    }

    public Task UpdateAuthorAsync(int authorId, UpsertAuthorRequest request, CancellationToken ct = default) =>
        ExecuteAsync("UPDATE Authors SET AuthorName=@name, Bio=@bio WHERE AuthorId=@id",
            [new("@id", authorId), new("@name", request.AuthorName), new("@bio", (object?)request.Bio ?? DBNull.Value)], ct);

    public Task DeleteAuthorAsync(int authorId, CancellationToken ct = default) =>
        ExecuteAsync("DELETE FROM Authors WHERE AuthorId=@id", [new("@id", authorId)], ct);

    public Task<IReadOnlyList<Category>> GetCategoriesAsync(CancellationToken ct = default) =>
        QueryAsync("SELECT CategoryId, CategoryName, Description, CreatedAt FROM Categories ORDER BY CategoryId DESC",
            r => new Category(r.GetInt32(0), r.GetString(1), r.IsDBNull(2) ? null : r.GetString(2), r.GetDateTime(3)), ct);

    public async Task<int> CreateCategoryAsync(UpsertCategoryRequest request, CancellationToken ct = default)
    {
        const string sql = "INSERT INTO Categories (CategoryName, Description) VALUES (@name,@desc); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", request.CategoryName);
        cmd.Parameters.AddWithValue("@desc", (object?)request.Description ?? DBNull.Value);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
    }

    public Task UpdateCategoryAsync(int categoryId, UpsertCategoryRequest request, CancellationToken ct = default) =>
        ExecuteAsync("UPDATE Categories SET CategoryName=@name, Description=@desc WHERE CategoryId=@id",
            [new("@id", categoryId), new("@name", request.CategoryName), new("@desc", (object?)request.Description ?? DBNull.Value)], ct);

    public Task DeleteCategoryAsync(int categoryId, CancellationToken ct = default) =>
        ExecuteAsync("DELETE FROM Categories WHERE CategoryId=@id", [new("@id", categoryId)], ct);

    public Task<IReadOnlyList<Customer>> GetCustomersAsync(CancellationToken ct = default) =>
        QueryAsync("SELECT CustomerId, FullName, Email, Phone, Address, CreatedAt FROM Customers ORDER BY CustomerId DESC",
            r => new Customer(r.GetInt32(0), r.GetString(1), DbString(r, 2), DbString(r, 3), DbString(r, 4), r.GetDateTime(5)), ct);

    public async Task<int> CreateCustomerAsync(UpsertCustomerRequest request, CancellationToken ct = default)
    {
        const string sql = @"INSERT INTO Customers (FullName, Email, Phone, Address)
VALUES (@name,@email,@phone,@address); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@name", request.FullName);
        cmd.Parameters.AddWithValue("@email", (object?)request.Email ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@phone", (object?)request.Phone ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@address", (object?)request.Address ?? DBNull.Value);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
    }

    public Task UpdateCustomerAsync(int customerId, UpsertCustomerRequest request, CancellationToken ct = default) =>
        ExecuteAsync("UPDATE Customers SET FullName=@name, Email=@email, Phone=@phone, Address=@address WHERE CustomerId=@id",
            [new("@id", customerId), new("@name", request.FullName), new("@email", (object?)request.Email ?? DBNull.Value), new("@phone", (object?)request.Phone ?? DBNull.Value), new("@address", (object?)request.Address ?? DBNull.Value)], ct);

    public Task DeleteCustomerAsync(int customerId, CancellationToken ct = default) =>
        ExecuteAsync("DELETE FROM Customers WHERE CustomerId=@id", [new("@id", customerId)], ct);

    public Task<IReadOnlyList<Book>> GetBooksAsync(CancellationToken ct = default) =>
        QueryAsync(@"SELECT b.BookId, b.Title, b.AuthorId, a.AuthorName, b.CategoryId, c.CategoryName, b.Price, b.Stock, b.PublishedYear, b.ISBN, b.CreatedAt
FROM Books b INNER JOIN Authors a ON b.AuthorId = a.AuthorId INNER JOIN Categories c ON b.CategoryId = c.CategoryId ORDER BY b.BookId DESC",
            r => new Book(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetString(3), r.GetInt32(4), r.GetString(5), r.GetDecimal(6), r.GetInt32(7), r.IsDBNull(8) ? null : r.GetInt32(8), DbString(r, 9), r.GetDateTime(10)), ct);

    public async Task<int> CreateBookAsync(UpsertBookRequest request, CancellationToken ct = default)
    {
        const string sql = @"INSERT INTO Books (Title, AuthorId, CategoryId, Price, Stock, PublishedYear, ISBN)
VALUES (@title,@authorId,@categoryId,@price,@stock,@year,@isbn); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@title", request.Title);
        cmd.Parameters.AddWithValue("@authorId", request.AuthorId);
        cmd.Parameters.AddWithValue("@categoryId", request.CategoryId);
        cmd.Parameters.AddWithValue("@price", request.Price);
        cmd.Parameters.AddWithValue("@stock", request.Stock);
        cmd.Parameters.AddWithValue("@year", (object?)request.PublishedYear ?? DBNull.Value);
        cmd.Parameters.AddWithValue("@isbn", (object?)request.Isbn ?? DBNull.Value);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
    }

    public Task UpdateBookAsync(int bookId, UpsertBookRequest request, CancellationToken ct = default) =>
        ExecuteAsync(@"UPDATE Books SET Title=@title, AuthorId=@authorId, CategoryId=@categoryId, Price=@price, Stock=@stock, PublishedYear=@year, ISBN=@isbn WHERE BookId=@id",
            [new("@id", bookId), new("@title", request.Title), new("@authorId", request.AuthorId), new("@categoryId", request.CategoryId), new("@price", request.Price), new("@stock", request.Stock), new("@year", (object?)request.PublishedYear ?? DBNull.Value), new("@isbn", (object?)request.Isbn ?? DBNull.Value)], ct);

    public Task DeleteBookAsync(int bookId, CancellationToken ct = default) =>
        ExecuteAsync("DELETE FROM Books WHERE BookId=@id", [new("@id", bookId)], ct);

    public Task<IReadOnlyList<Order>> GetOrdersAsync(CancellationToken ct = default) =>
        QueryAsync(@"SELECT o.OrderId, o.CustomerId, c.FullName, o.OrderDate, o.TotalAmount, o.Status
FROM Orders o INNER JOIN Customers c ON o.CustomerId = c.CustomerId ORDER BY o.OrderId DESC",
            r => new Order(r.GetInt32(0), r.GetInt32(1), r.GetString(2), r.GetDateTime(3), r.GetDecimal(4), r.GetString(5)), ct);

    public async Task<int> CreateOrderAsync(CreateOrderRequest request, CancellationToken ct = default)
    {
        const string sql = "INSERT INTO Orders (CustomerId, TotalAmount, Status) VALUES (@customerId,0,'Pending'); SELECT CAST(SCOPE_IDENTITY() AS INT);";
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@customerId", request.CustomerId);
        return Convert.ToInt32(await cmd.ExecuteScalarAsync(ct));
    }

    public async Task AddOrderItemAsync(AddOrderItemRequest request, CancellationToken ct = default)
    {
        const string sql = @"BEGIN TRAN
INSERT INTO OrderDetails (OrderId, BookId, Quantity, UnitPrice)
SELECT @orderId, b.BookId, @quantity, b.Price FROM Books b WHERE b.BookId = @bookId;
UPDATE Books SET Stock = Stock - @quantity WHERE BookId = @bookId AND Stock >= @quantity;
UPDATE Orders SET TotalAmount = ISNULL((SELECT SUM(SubTotal) FROM OrderDetails WHERE OrderId = @orderId),0) WHERE OrderId = @orderId;
COMMIT";
        await ExecuteAsync(sql,
            [new("@orderId", request.OrderId), new("@bookId", request.BookId), new("@quantity", request.Quantity)], ct);
    }

    public Task UpdateOrderStatusAsync(UpdateOrderStatusRequest request, CancellationToken ct = default) =>
        ExecuteAsync("UPDATE Orders SET Status=@status WHERE OrderId=@id", [new("@id", request.OrderId), new("@status", request.Status)], ct);

    public async Task CancelOrderAsync(int orderId, CancellationToken ct = default)
    {
        const string sql = @"BEGIN TRAN
UPDATE b SET b.Stock = b.Stock + od.Quantity FROM Books b INNER JOIN OrderDetails od ON od.BookId=b.BookId WHERE od.OrderId=@orderId;
UPDATE Orders SET Status='Cancelled' WHERE OrderId=@orderId;
COMMIT";
        await ExecuteAsync(sql, [new("@orderId", orderId)], ct);
    }

    public Task<IReadOnlyList<OrderItem>> GetOrderItemsAsync(int orderId, CancellationToken ct = default) =>
        QueryAsync(@"SELECT od.OrderDetailId, od.OrderId, od.BookId, b.Title, od.Quantity, od.UnitPrice, od.SubTotal
FROM OrderDetails od INNER JOIN Books b ON od.BookId = b.BookId WHERE od.OrderId = @orderId",
            r => new OrderItem(r.GetInt32(0), r.GetInt32(1), r.GetInt32(2), r.GetString(3), r.GetInt32(4), r.GetDecimal(5), r.GetDecimal(6)), ct,
            new SqlParameter("@orderId", orderId));

    public async Task MakePaymentAsync(MakePaymentRequest request, CancellationToken ct = default)
    {
        const string sql = @"BEGIN TRAN
INSERT INTO Payments (OrderId, Amount, PaymentMethod) VALUES (@orderId,@amount,@method);
UPDATE Orders SET Status='Paid' WHERE OrderId=@orderId;
COMMIT";
        await ExecuteAsync(sql,
            [new("@orderId", request.OrderId), new("@amount", request.Amount), new("@method", request.PaymentMethod)], ct);
    }

    public Task<IReadOnlyList<Payment>> GetPaymentsByOrderAsync(int orderId, CancellationToken ct = default) =>
        QueryAsync("SELECT PaymentId, OrderId, PaymentDate, Amount, PaymentMethod FROM Payments WHERE OrderId=@orderId ORDER BY PaymentDate DESC",
            r => new Payment(r.GetInt32(0), r.GetInt32(1), r.GetDateTime(2), r.GetDecimal(3), DbString(r, 4)), ct,
            new SqlParameter("@orderId", orderId));

    public Task<IReadOnlyList<SalesReportRow>> GetSalesReportAsync(CancellationToken ct = default) =>
        QueryAsync("SELECT CAST(PaymentDate AS DATE) AS [Date], SUM(Amount) AS TotalSales, COUNT(*) AS PaymentCount FROM Payments GROUP BY CAST(PaymentDate AS DATE) ORDER BY [Date] DESC",
            r => new SalesReportRow(r.GetDateTime(0), r.GetDecimal(1), r.GetInt32(2)), ct);

    public Task<IReadOnlyList<StockReportRow>> GetStockReportAsync(CancellationToken ct = default) =>
        QueryAsync(@"SELECT b.BookId, b.Title, b.Stock, b.Price, a.AuthorName, c.CategoryName
FROM Books b INNER JOIN Authors a ON b.AuthorId = a.AuthorId INNER JOIN Categories c ON b.CategoryId = c.CategoryId ORDER BY b.Stock ASC, b.Title",
            r => new StockReportRow(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetDecimal(3), r.GetString(4), r.GetString(5)), ct);

    public Task<IReadOnlyList<CustomerOrderSummaryRow>> GetCustomerOrderSummaryAsync(CancellationToken ct = default) =>
        QueryAsync(@"SELECT c.CustomerId, c.FullName, COUNT(o.OrderId) AS TotalOrders, ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Customers c LEFT JOIN Orders o ON c.CustomerId = o.CustomerId GROUP BY c.CustomerId, c.FullName ORDER BY TotalSpent DESC",
            r => new CustomerOrderSummaryRow(r.GetInt32(0), r.GetString(1), r.GetInt32(2), r.GetDecimal(3)), ct);

    private async Task ExecuteAsync(string sql, IEnumerable<SqlParameter> parameters, CancellationToken ct)
    {
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddRange(parameters.ToArray());
        await cmd.ExecuteNonQueryAsync(ct);
    }

    private async Task<IReadOnlyList<T>> QueryAsync<T>(string sql, Func<SqlDataReader, T> map, CancellationToken ct, params SqlParameter[] parameters)
    {
        var list = new List<T>();
        await using var conn = Connection();
        await conn.OpenAsync(ct);
        await using var cmd = new SqlCommand(sql, conn);
        if (parameters.Length > 0) cmd.Parameters.AddRange(parameters);
        await using var reader = await cmd.ExecuteReaderAsync(ct);
        while (await reader.ReadAsync(ct)) list.Add(map(reader));
        return list;
    }

    private static string? DbString(SqlDataReader r, int index) => r.IsDBNull(index) ? null : r.GetString(index);
}
