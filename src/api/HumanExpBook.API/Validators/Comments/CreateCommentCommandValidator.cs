using FluentValidation;
using HumanExpBook.BLL.Commands.Posts;

namespace HumanExpBook.API.Validators.Comments;

public sealed class CreateCommentCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(q => q.CurrentUserId)
            .NotEmpty();

        RuleFor(q => q.Content)
            .NotEmpty();
    }
}