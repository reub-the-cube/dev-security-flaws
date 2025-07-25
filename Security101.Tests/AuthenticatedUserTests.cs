using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Security101.Tests;

[Collection("Sequential")]
public class AuthenticatedUserTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticatedUserTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithAuthentication<TestUserAuthHandler>();
        _httpClient = _factory.CreateClient();

        TestData.AddSeededData(_factory.Services);
    }

    public void Dispose()
    {
        TestData.RemoveSeededData(_factory.Services);
    }

    [Fact]
    public async Task AuthenticatedUserCanGetAToDoItem()
    {
        var response = await _httpClient.GetAsync("/api/todoitems/1");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task AuthenticatedUserCannotGetAllItems()
    {
        var response = await _httpClient.GetAsync("/api/todoitems");

        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }
}