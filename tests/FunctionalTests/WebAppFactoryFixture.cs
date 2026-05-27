using Microsoft.AspNetCore.Mvc.Testing;

public class WebAppFactoryFixture : WebApplicationFactory<Program>
{
    public WebAppFactoryFixture()
    {
        Client = CreateClient();
    }

    public HttpClient Client { get; }
}