namespace IPB2.OnlineBookStoreSystem.Mvc.Features.Categories;

public record CategoryResponse(int CategoryId, string CategoryName, string? Description, DateTime CreatedAt);
public record UpsertCategoryRequest(string CategoryName, string? Description);
