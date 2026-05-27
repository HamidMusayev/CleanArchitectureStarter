using System.Net.Http.Json;
using Contracts.v1.Users;

namespace IntegrationTests.Api.Users;

// Tests HTTP endpoint workflow.
// (Assume a standard WebAppFactoryFixture using WebApplicationFactory<Program> if you scaffold tests.)
public class CreateUserEndpointTests : IClassFixture<WebAppFactoryFixture>
{
    private readonly WebAppFactoryFixture _fx;

    public CreateUserEndpointTests(WebAppFactoryFixture fx)
    {
        _fx = fx;
    }

    [Fact]
    public async Task Post_Creates_User()
    {
        var client = _fx.Client;
        var res = await client.PostAsJsonAsync("/api/v1/users", new CreateUserRequest("a@b.com", "Ada", "Lovelace"));
        res.EnsureSuccessStatusCode();
        var dto = await res.Content.ReadFromJsonAsync<UserResponse>();
        Assert.NotNull(dto);
    }
}