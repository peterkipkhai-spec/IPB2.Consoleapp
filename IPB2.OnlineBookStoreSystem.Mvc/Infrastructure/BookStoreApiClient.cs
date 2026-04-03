using System.Net.Http.Json;

namespace IPB2.OnlineBookStoreSystem.Mvc.Infrastructure;

public sealed class BookStoreApiClient(HttpClient httpClient)
{
    public async Task<IReadOnlyList<T>> GetListAsync<T>(string path, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<IReadOnlyList<T>>(path, ct) ?? [];

    public async Task<T?> GetAsync<T>(string path, CancellationToken ct = default)
        => await httpClient.GetFromJsonAsync<T>(path, ct);

    public async Task PostAsync<TRequest>(string path, TRequest request, CancellationToken ct = default)
    {
        using var response = await httpClient.PostAsJsonAsync(path, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync<TRequest>(string path, TRequest request, CancellationToken ct = default)
    {
        using var response = await httpClient.PutAsJsonAsync(path, request, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task PutAsync(string path, CancellationToken ct = default)
    {
        using var response = await httpClient.PutAsync(path, null, ct);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(string path, CancellationToken ct = default)
    {
        using var response = await httpClient.DeleteAsync(path, ct);
        response.EnsureSuccessStatusCode();
    }
}
