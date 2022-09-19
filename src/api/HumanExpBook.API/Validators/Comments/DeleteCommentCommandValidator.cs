using FluentValidation;
using HumanExpBook.BLL.Commands.Comments;

namespace HumanExpBook.API.Validators.Comments;

public sealed class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
        => RuleFor(q => q.Id)
            .NotEmpty();
}