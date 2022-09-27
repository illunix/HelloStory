using FluentValidation;
using HelloStory.Posts.BLL.Commands;

namespace HelloStory.Posts.API.Validators;

internal sealed class LikePostCommandValidator : AbstractValidator<LikePostCommand>
{
    public LikePostCommandValidator()
    {
        RuleFor(q => q.PostId)
            .NotEmpty();

        RuleFor(q => q.UserId)
            .NotEmpty();
    }
}