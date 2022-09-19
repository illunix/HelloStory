using FluentValidation;
using HumanExpBook.BLL.Commands.Authflow;

namespace HumanExpBook.API.Validators.Authflow;

public class RevokeAccessTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeAccessTokenCommandValidator()
    {
        RuleFor(q => q.CurrentUserId)
            .NotEmpty();
    }
}