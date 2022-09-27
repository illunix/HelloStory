using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Posts.BLL.Commands;

public readonly record struct LikePostCommand(
    Guid PostId,
    Guid UserId
) : IHttpRequest;