namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Authors;

public record AuthorResponse(int AuthorId, string AuthorName, string? Bio, DateTime CreatedAt);
public record UpsertAuthorRequest(string AuthorName, string? Bio);
