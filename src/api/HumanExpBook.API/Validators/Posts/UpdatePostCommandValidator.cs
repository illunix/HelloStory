using FluentValidation;
using HumanExpBook.BLL.Commands.Posts;

namespace HumanExpBook.API.Validators.Posts;

public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();

        RuleFor(q => q.Content)
            .MinimumLength(50);
    }
}