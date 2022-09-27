using FluentValidation;
using HelloStory.Authflow.BLL.Commands;

namespace HelloStory.Authflow.API.Validators;

internal sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(q => q.EmailOrUsername)
            .NotEmpty();

        RuleFor(q => q.Password)
            .NotEmpty();
    }
}