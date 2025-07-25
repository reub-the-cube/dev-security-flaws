using System.Net;

namespace Security101.Tests;

[Collection("Sequential")]
public class UnauthenticatedUserTests(TestWebApplicationFactory<Program> factory) : IClassFixture<TestWebApplicationFactory<Program>>
{
    private readonly TestWebApplicationFactory<Program> _factory = factory;
    private readonly HttpClient _httpClient = factory.CreateClient();

    [Fact]
    public async Task UnauthenticatedUserCannotGetAToDoItem()
    {
        var response = await _httpClient.GetAsync("/api/todoitems/1");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}