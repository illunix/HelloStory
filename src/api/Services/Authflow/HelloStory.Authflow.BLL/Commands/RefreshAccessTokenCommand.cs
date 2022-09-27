using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Authflow.BLL.Commands;

public readonly record struct RefreshAccessTokenCommand(Guid CurrentUserId) : IHttpRequest;