using FluentValidation;
using HumanExpBook.BLL.Commands.Authflow;

namespace HumanExpBook.API.Validators.Authflow;

public class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
    {
        RuleFor(q => q.RefreshToken)
            .NotEmpty();

        RuleFor(q => q.CurrentUserId)
            .NotEmpty();
    }
}