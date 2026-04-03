namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Customers;

public record CustomerResponse(int CustomerId, string FullName, string? Email, string? Phone, string? Address, DateTime CreatedAt);
public record UpsertCustomerRequest(string FullName, string? Email, string? Phone, string? Address);
