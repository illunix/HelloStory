using FluentValidation;
using HelloStory.Authflow.BLL.Commands;

namespace HelloStory.Authflow.API.Validators;

internal sealed class RefreshAccessTokenCommandValidator : AbstractValidator<RefreshAccessTokenCommand>
{
    public RefreshAccessTokenCommandValidator()
        => RuleFor(q => q.CurrentUserId)
            .NotEmpty();
}
