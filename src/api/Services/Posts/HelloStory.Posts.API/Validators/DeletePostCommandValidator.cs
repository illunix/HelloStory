using FluentValidation;
using HelloStory.Posts.BLL.Commands;

namespace HelloStory.Posts.API.Validators;

internal sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
        => RuleFor(q => q.Id)
            .NotEmpty();
}
