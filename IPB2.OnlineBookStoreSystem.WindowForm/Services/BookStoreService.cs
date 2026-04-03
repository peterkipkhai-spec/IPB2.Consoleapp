using System.Data;
using Microsoft.Data.SqlClient;
using IPB2.OnlineBookStoreSystem.WindowForm.Data;

namespace IPB2.OnlineBookStoreSystem.WindowForm.Services;

public sealed class BookStoreService
{
    private readonly SqlDb _db = new();

    public DataTable GetAuthors() => _db.Query("SELECT AuthorId, AuthorName, Bio, CreatedAt FROM Authors ORDER BY AuthorId DESC");

    public void CreateAuthor(string name, string? bio)
    {
        _db.Execute("INSERT INTO Authors (AuthorName, Bio) VALUES (@name, @bio)",
            new SqlParameter("@name", name),
            new SqlParameter("@bio", (object?)bio ?? DBNull.Value));
    }

    public void UpdateAuthor(int id, string name, string? bio)
    {
        _db.Execute("UPDATE Authors SET AuthorName=@name, Bio=@bio WHERE AuthorId=@id",
            new SqlParameter("@id", id),
            new SqlParameter("@name", name),
            new SqlParameter("@bio", (object?)bio ?? DBNull.Value));
    }

    public void DeleteAuthor(int id) =>
        _db.Execute("DELETE FROM Authors WHERE AuthorId=@id", new SqlParameter("@id", id));

    public DataTable GetCategories() => _db.Query("SELECT CategoryId, CategoryName, Description, CreatedAt FROM Categories ORDER BY CategoryId DESC");

    public void CreateCategory(string name, string? description)
    {
        _db.Execute("INSERT INTO Categories (CategoryName, Description) VALUES (@name, @description)",
            new SqlParameter("@name", name),
            new SqlParameter("@description", (object?)description ?? DBNull.Value));
    }

    public void UpdateCategory(int id, string name, string? description)
    {
        _db.Execute("UPDATE Categories SET CategoryName=@name, Description=@description WHERE CategoryId=@id",
            new SqlParameter("@id", id),
            new SqlParameter("@name", name),
            new SqlParameter("@description", (object?)description ?? DBNull.Value));
    }

    public void DeleteCategory(int id) =>
        _db.Execute("DELETE FROM Categories WHERE CategoryId=@id", new SqlParameter("@id", id));

    public DataTable GetCustomers() => _db.Query("SELECT CustomerId, FullName, Email, Phone, Address, CreatedAt FROM Customers ORDER BY CustomerId DESC");

    public void CreateCustomer(string fullName, string? email, string? phone, string? address)
    {
        _db.Execute("INSERT INTO Customers (FullName, Email, Phone, Address) VALUES (@fullName, @email, @phone, @address)",
            new SqlParameter("@fullName", fullName),
            new SqlParameter("@email", (object?)email ?? DBNull.Value),
            new SqlParameter("@phone", (object?)phone ?? DBNull.Value),
            new SqlParameter("@address", (object?)address ?? DBNull.Value));
    }

    public void UpdateCustomer(int id, string fullName, string? email, string? phone, string? address)
    {
        _db.Execute("UPDATE Customers SET FullName=@fullName, Email=@email, Phone=@phone, Address=@address WHERE CustomerId=@id",
            new SqlParameter("@id", id),
            new SqlParameter("@fullName", fullName),
            new SqlParameter("@email", (object?)email ?? DBNull.Value),
            new SqlParameter("@phone", (object?)phone ?? DBNull.Value),
            new SqlParameter("@address", (object?)address ?? DBNull.Value));
    }

    public void DeleteCustomer(int id) =>
        _db.Execute("DELETE FROM Customers WHERE CustomerId=@id", new SqlParameter("@id", id));

    public DataTable GetBooks() => _db.Query(@"
SELECT b.BookId, b.Title, b.AuthorId, a.AuthorName, b.CategoryId, c.CategoryName, b.Price, b.Stock, b.PublishedYear, b.ISBN, b.CreatedAt
FROM Books b
INNER JOIN Authors a ON b.AuthorId = a.AuthorId
INNER JOIN Categories c ON b.CategoryId = c.CategoryId
ORDER BY b.BookId DESC");

    public DataTable GetAuthorLookup() => _db.Query("SELECT AuthorId, AuthorName FROM Authors ORDER BY AuthorName");
    public DataTable GetCategoryLookup() => _db.Query("SELECT CategoryId, CategoryName FROM Categories ORDER BY CategoryName");
    public DataTable GetBookLookup() => _db.Query("SELECT BookId, Title, Price, Stock FROM Books ORDER BY Title");
    public DataTable GetCustomerLookup() => _db.Query("SELECT CustomerId, FullName FROM Customers ORDER BY FullName");

    public void CreateBook(string title, int authorId, int categoryId, decimal price, int stock, int? publishedYear, string? isbn)
    {
        _db.Execute(@"INSERT INTO Books (Title, AuthorId, CategoryId, Price, Stock, PublishedYear, ISBN)
VALUES (@title, @authorId, @categoryId, @price, @stock, @publishedYear, @isbn)",
            new SqlParameter("@title", title),
            new SqlParameter("@authorId", authorId),
            new SqlParameter("@categoryId", categoryId),
            new SqlParameter("@price", price),
            new SqlParameter("@stock", stock),
            new SqlParameter("@publishedYear", (object?)publishedYear ?? DBNull.Value),
            new SqlParameter("@isbn", (object?)isbn ?? DBNull.Value));
    }

    public void UpdateBook(int id, string title, int authorId, int categoryId, decimal price, int stock, int? publishedYear, string? isbn)
    {
        _db.Execute(@"UPDATE Books
SET Title=@title, AuthorId=@authorId, CategoryId=@categoryId, Price=@price, Stock=@stock, PublishedYear=@publishedYear, ISBN=@isbn
WHERE BookId=@id",
            new SqlParameter("@id", id),
            new SqlParameter("@title", title),
            new SqlParameter("@authorId", authorId),
            new SqlParameter("@categoryId", categoryId),
            new SqlParameter("@price", price),
            new SqlParameter("@stock", stock),
            new SqlParameter("@publishedYear", (object?)publishedYear ?? DBNull.Value),
            new SqlParameter("@isbn", (object?)isbn ?? DBNull.Value));
    }

    public void DeleteBook(int id) =>
        _db.Execute("DELETE FROM Books WHERE BookId=@id", new SqlParameter("@id", id));

    public int CreateOrder(int customerId)
    {
        var id = _db.Scalar(@"INSERT INTO Orders (CustomerId, TotalAmount, Status)
VALUES (@customerId, 0, 'Pending');
SELECT CAST(SCOPE_IDENTITY() AS INT);", new SqlParameter("@customerId", customerId));
        return Convert.ToInt32(id);
    }

    public void AddOrderItem(int orderId, int bookId, int quantity)
    {
        _db.Execute(@"
INSERT INTO OrderDetails (OrderId, BookId, Quantity, UnitPrice)
SELECT @orderId, b.BookId, @quantity, b.Price
FROM Books b
WHERE b.BookId = @bookId;

UPDATE Books
SET Stock = Stock - @quantity
WHERE BookId = @bookId AND Stock >= @quantity;

UPDATE Orders
SET TotalAmount = ISNULL((SELECT SUM(SubTotal) FROM OrderDetails WHERE OrderId = @orderId), 0)
WHERE OrderId = @orderId;",
            new SqlParameter("@orderId", orderId),
            new SqlParameter("@bookId", bookId),
            new SqlParameter("@quantity", quantity));
    }

    public void UpdateOrderStatus(int orderId, string status)
    {
        _db.Execute("UPDATE Orders SET Status=@status WHERE OrderId=@orderId",
            new SqlParameter("@orderId", orderId),
            new SqlParameter("@status", status));
    }

    public void CancelOrder(int orderId)
    {
        _db.Execute(@"
UPDATE b
SET b.Stock = b.Stock + od.Quantity
FROM Books b
INNER JOIN OrderDetails od ON od.BookId = b.BookId
WHERE od.OrderId = @orderId;

UPDATE Orders SET Status='Cancelled' WHERE OrderId=@orderId;",
            new SqlParameter("@orderId", orderId));
    }

    public DataTable GetOrders() => _db.Query(@"
SELECT o.OrderId, o.CustomerId, c.FullName, o.OrderDate, o.TotalAmount, o.Status
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.CustomerId
ORDER BY o.OrderId DESC");

    public DataTable GetOrderItems(int orderId) => _db.Query(@"
SELECT od.OrderDetailId, od.OrderId, od.BookId, b.Title, od.Quantity, od.UnitPrice, od.SubTotal
FROM OrderDetails od
INNER JOIN Books b ON od.BookId = b.BookId
WHERE od.OrderId = @orderId",
        new SqlParameter("@orderId", orderId));

    public void MakePayment(int orderId, decimal amount, string paymentMethod)
    {
        _db.Execute(@"INSERT INTO Payments (OrderId, Amount, PaymentMethod) VALUES (@orderId, @amount, @method)",
            new SqlParameter("@orderId", orderId),
            new SqlParameter("@amount", amount),
            new SqlParameter("@method", paymentMethod));

        _db.Execute("UPDATE Orders SET Status='Paid' WHERE OrderId=@orderId", new SqlParameter("@orderId", orderId));
    }

    public DataTable GetSalesReport() => _db.Query(@"
SELECT CAST(PaymentDate AS DATE) AS [Date], SUM(Amount) AS TotalSales, COUNT(*) AS PaymentCount
FROM Payments
GROUP BY CAST(PaymentDate AS DATE)
ORDER BY [Date] DESC");

    public DataTable GetStockReport() => _db.Query(@"
SELECT b.BookId, b.Title, b.Stock, b.Price, a.AuthorName, c.CategoryName
FROM Books b
INNER JOIN Authors a ON b.AuthorId = a.AuthorId
INNER JOIN Categories c ON b.CategoryId = c.CategoryId
ORDER BY b.Stock ASC, b.Title");

    public DataTable GetCustomerOrderSummary() => _db.Query(@"
SELECT c.CustomerId, c.FullName, COUNT(o.OrderId) AS TotalOrders, ISNULL(SUM(o.TotalAmount), 0) AS TotalSpent
FROM Customers c
LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
GROUP BY c.CustomerId, c.FullName
ORDER BY TotalSpent DESC");

    public DataTable GetPaymentsByOrder(int orderId) => _db.Query(@"
SELECT PaymentId, OrderId, PaymentDate, Amount, PaymentMethod
FROM Payments
WHERE OrderId = @orderId
ORDER BY PaymentDate DESC", new SqlParameter("@orderId", orderId));
}
