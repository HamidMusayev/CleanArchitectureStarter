using Application.Abstractions.Messaging;

namespace Infrastructure.Services;

public sealed class EmailSender : IEmailSender
{
    public Task SendAsync(string to, string subject, string body, CancellationToken ct = default)
    {
        // TODO: plug in SMTP/provider
        Console.WriteLine($"Email to {to}: {subject}");
        return Task.CompletedTask;
    }
}
