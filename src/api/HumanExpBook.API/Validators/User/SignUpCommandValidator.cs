using FluentValidation;
using HumanExpBook.BLL.Commands.User;

namespace HumanExpBook.API.Validators.User;

public sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
{
    public SignUpCommandValidator()
    {
        RuleFor(q => q.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(q => q.Username)
            .NotEmpty();

        RuleFor(q => q.Password)
            .NotEmpty();

        RuleFor(q => q.ConfirmPassword)
            .NotEmpty()
            .Equal(q => q.Password);
    }
}