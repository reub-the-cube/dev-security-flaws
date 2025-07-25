using Microsoft.AspNetCore.Mvc.Testing;
using Security101.Models;
using System.Net;
using System.Net.Http.Json;

namespace Security101.Tests;

[Collection("Sequential")]
public class PostToDoItemTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public PostToDoItemTests(TestWebApplicationFactory<Program> factory)
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
    public async Task AuthenticatedUserCanAddAToDoItemAndTheAuthorIsRecorded()
    {
        var itemToAdd = new TodoItem
        {
            Id = 4,
            Name = "New to do item",
            IsComplete = true,
            AuthoredBy = "Anything"
        };

        var response = await _httpClient.PostAsJsonAsync("/api/todoitems", itemToAdd);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);

        var createdItem = await response.Content.ReadFromJsonAsync<TodoItem>();
        Assert.Equal(4, createdItem?.Id);
        Assert.Equal(itemToAdd.Name, createdItem?.Name);
        Assert.Equal(itemToAdd.IsComplete, createdItem?.IsComplete);
        Assert.Equal(TestData.TEST_USER_NAME, createdItem?.AuthoredBy);
    }
}