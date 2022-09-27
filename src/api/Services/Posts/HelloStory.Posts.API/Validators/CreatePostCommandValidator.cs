using FluentValidation;
using HelloStory.Posts.BLL.Commands;

namespace HelloStory.Posts.API.Validators;

internal sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(q => q.UserId)
            .NotEmpty();

        RuleFor(q => q.Content)
            .MinimumLength(50)
            .MaximumLength(2000);
    }
}