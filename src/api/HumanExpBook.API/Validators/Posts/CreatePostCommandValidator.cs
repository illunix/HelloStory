using FluentValidation;
using HumanExpBook.BLL.Commands.Posts;

namespace HumanExpBook.API.Validators.Posts;

public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(q => q.CurrentUserId)
            .NotEmpty();

        RuleFor(q => q.Content)
            .MinimumLength(50);
    }
}