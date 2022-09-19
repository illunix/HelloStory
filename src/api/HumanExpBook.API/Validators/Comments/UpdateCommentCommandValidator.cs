using FluentValidation;
using HumanExpBook.BLL.Commands.Comments;

namespace HumanExpBook.API.Validators.Comments;

public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();

        RuleFor(q => q.Content)
            .NotEmpty();
    }
}