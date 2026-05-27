using Application.Abstractions.Messaging;
using Domain.Users.Events;
using MediatR;

namespace Application.Features.Users.Events;

public sealed class UserCreatedDomainEventHandler(IEmailSender emailSender)
    : INotificationHandler<UserCreatedDomainEvent>
{
    public Task Handle(UserCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        return emailSender.SendAsync(
            notification.Email,
            "Welcome!",
            $"Your account was created on {notification.CreatedAt:u}.",
            cancellationToken);
    }
}