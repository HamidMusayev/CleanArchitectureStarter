using FluentValidation;

namespace Application.Features.Users.Commands.CreateUser;

public sealed class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        RuleFor(x => x.Password).NotEmpty().MinimumLength(8).MaximumLength(128);
        RuleFor(x => x.GivenName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FamilyName).NotEmpty().MaximumLength(100);
    }
}
