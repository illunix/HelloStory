using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.User.BLL.Commands;

public readonly record struct SignUpCommand(
    string Email,
    string Username,
    string Password,
    string ConfirmPassword
) : IHttpRequest;