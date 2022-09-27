using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Posts.BLL.Commands;

public readonly record struct CreatePostCommand(
    Guid UserId,
    string Content
) : IHttpRequest;