using FluentValidation;
using HelloStory.User.BLL.Commands;

namespace HelloStory.User.API.Validators.Command;

internal sealed class SignUpCommandValidator : AbstractValidator<SignUpCommand>
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