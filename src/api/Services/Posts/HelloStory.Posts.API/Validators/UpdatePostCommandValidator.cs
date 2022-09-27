using FluentValidation;
using HelloStory.Posts.BLL.Commands;

namespace HelloStory.Posts.API.Validators;

internal sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(q => q.Id)
            .NotEmpty();

        RuleFor(q => q.Content)
            .MinimumLength(50)
            .MaximumLength(2000);
    }
}