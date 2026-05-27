using Microsoft.AspNetCore.Mvc.Testing;

namespace IntegrationTests;

public class WebAppFactoryFixture : WebApplicationFactory<Program>
{
    public WebAppFactoryFixture()
    {
        Client = CreateClient();
    }

    public HttpClient Client { get; }
}