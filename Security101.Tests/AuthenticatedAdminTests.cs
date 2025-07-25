using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;

namespace Security101.Tests;

[Collection("Sequential")]
public class AuthenticatedAdminTests : IClassFixture<TestWebApplicationFactory<Program>>, IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticatedAdminTests(TestWebApplicationFactory<Program> factory)
    {
        _factory = factory.WithAuthentication<TestAdminAuthHandler>();
        _httpClient = _factory.CreateClient();

        TestData.AddSeededData(_factory.Services);
    }

    public void Dispose()
    {
        TestData.RemoveToDoItems(_factory.Services);
    }

    [Fact]
    public async Task AuthenticatedUserCanGetAllItems()
    {
        var response = await _httpClient.GetAsync("/api/todoitems");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}