using FluentValidation;

namespace Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
        RuleFor(x => x.GivenName).NotEmpty().MaximumLength(100);
        RuleFor(x => x.FamilyName).NotEmpty().MaximumLength(100);
    }
}
