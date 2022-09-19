using FluentValidation;
using HumanExpBook.BLL.Commands.Posts;

namespace HumanExpBook.API.Validators.Posts;

public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();
    }
}