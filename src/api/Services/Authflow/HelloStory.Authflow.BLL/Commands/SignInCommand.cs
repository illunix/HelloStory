using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Authflow.BLL.Commands;

public readonly record struct SignInCommand(
    string EmailOrUsername,
    string Password
) : IHttpRequest;