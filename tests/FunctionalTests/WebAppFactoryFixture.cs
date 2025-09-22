using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;

// `Program` should be the entry point of your API project (the one with `Main`).
public class WebAppFactoryFixture : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }

    public WebAppFactoryFixture()
    {
        // This creates an in-memory test server and HttpClient
        // base address will be set automatically
        Client = CreateClient();
    }
}