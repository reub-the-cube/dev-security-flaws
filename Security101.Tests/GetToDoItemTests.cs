using Microsoft.AspNetCore.Mvc.Testing;
using Security101.Models;
using System.Net;
using System.Net.Http.Json;

namespace Security101.Tests;

[Collection("Sequential")]
public class GetToDoItemTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public GetToDoItemTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithAuthentication<TestUserAuthHandler>();
        _httpClient = _factory.CreateClient();

        TestData.AddSeededData(_factory.Services);
    }

    public void Dispose()
    {
        TestData.RemoveToDoItems(_factory.Services);
    }

    [Fact]
    public async Task AuthenticatedUserCannotGetAnItemThatDoesNotExist()
    {
        var response = await _httpClient.GetAsync("/api/todoitems/0");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticatedUserCanGetItemsThatMatchAString()
    {
        var response = await _httpClient.GetFromJsonAsync<List<TodoItem>>("/api/todoitems/name/some");

        Assert.Equal(3, response?.Count);
    }
}