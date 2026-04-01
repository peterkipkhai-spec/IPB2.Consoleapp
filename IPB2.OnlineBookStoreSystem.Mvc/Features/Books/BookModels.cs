namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Books;

public record BookResponse(int BookId, string Title, int AuthorId, string AuthorName, int CategoryId, string CategoryName, decimal Price, int Stock, int? PublishedYear, string? Isbn, DateTime CreatedAt);
public record UpsertBookRequest(string Title, int AuthorId, int CategoryId, decimal Price, int Stock, int? PublishedYear, string? Isbn);
