using System.Net;
using Xunit;

// Replace Program with the actual entry point class of your web app
namespace FunctionalTests;

public class BasicSmokeTests : IClassFixture<WebAppFactoryFixture>
{
    private readonly WebAppFactoryFixture _fx;
    public BasicSmokeTests(WebAppFactoryFixture fx) => _fx = fx;

    [Fact]
    public async Task Health_Returns_Ok()
    {
        var res = await _fx.Client.GetAsync("/health");
        Assert.Equal(HttpStatusCode.OK, res.StatusCode);
    }
}