using FluentValidation;
using HelloStory.Authflow.BLL.Commands;

namespace HelloStory.Authflow.API.Validators;

internal sealed class RevokeAccessTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeAccessTokenCommandValidator()
        => RuleFor(q => q.CurrentUserId)
            .NotEmpty();
}
