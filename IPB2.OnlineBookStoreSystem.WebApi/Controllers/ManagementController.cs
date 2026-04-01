using IPB2.OnlineBookStoreSystem.Domain.Contracts;
using IPB2.OnlineBookStoreSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IPB2.OnlineBookStoreSystem.WebApi.Controllers;

[ApiController]
[Route("api/management")]
public sealed class ManagementController(IBookStoreService service) : ControllerBase
{
    [HttpGet("authors")]
    public async Task<IActionResult> GetAuthors(CancellationToken ct) => Ok(await service.GetAuthorsAsync(ct));

    [HttpPost("authors")]
    public async Task<IActionResult> CreateAuthor([FromBody] UpsertAuthorRequest request, CancellationToken ct)
        => Ok(new { id = await service.CreateAuthorAsync(request, ct) });

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories(CancellationToken ct) => Ok(await service.GetCategoriesAsync(ct));

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] UpsertCategoryRequest request, CancellationToken ct)
        => Ok(new { id = await service.CreateCategoryAsync(request, ct) });

    [HttpGet("customers")]
    public async Task<IActionResult> GetCustomers(CancellationToken ct) => Ok(await service.GetCustomersAsync(ct));

    [HttpPost("customers")]
    public async Task<IActionResult> CreateCustomer([FromBody] UpsertCustomerRequest request, CancellationToken ct)
        => Ok(new { id = await service.CreateCustomerAsync(request, ct) });

    [HttpGet("books")]
    public async Task<IActionResult> GetBooks(CancellationToken ct) => Ok(await service.GetBooksAsync(ct));

    [HttpPost("books")]
    public async Task<IActionResult> CreateBook([FromBody] UpsertBookRequest request, CancellationToken ct)
        => Ok(new { id = await service.CreateBookAsync(request, ct) });
}
