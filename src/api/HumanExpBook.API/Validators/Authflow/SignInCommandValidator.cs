using FluentValidation;
using HumanExpBook.BLL.Commands.Authflow;

namespace HumanExpBook.API.Validators.Authflow;

public sealed class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(q => q.EmailOrUsername)
            .NotEmpty();

        RuleFor(q => q.Password)
            .NotEmpty();
    }
}