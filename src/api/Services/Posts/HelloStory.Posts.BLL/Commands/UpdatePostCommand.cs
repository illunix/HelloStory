using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Posts.BLL.Commands;

public readonly record struct UpdatePostCommand(
    Guid Id,
    string Content
) : IHttpRequest;