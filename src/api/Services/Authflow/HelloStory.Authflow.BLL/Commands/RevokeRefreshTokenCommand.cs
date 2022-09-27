using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Authflow.BLL.Commands;

public readonly record struct RevokeRefreshTokenCommand(Guid CurrentUserId) : IHttpRequest;