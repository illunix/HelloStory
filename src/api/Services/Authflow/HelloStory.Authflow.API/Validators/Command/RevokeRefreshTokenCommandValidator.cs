using FluentValidation;
using HelloStory.Authflow.BLL.Commands;

namespace HelloStory.Authflow.API.Validators.Command;

internal sealed class RevokeRefreshTokenCommandValidator : AbstractValidator<RevokeRefreshTokenCommand>
{
    public RevokeRefreshTokenCommandValidator()
        => RuleFor(q => q.CurrentUserId)
            .NotEmpty();
}
