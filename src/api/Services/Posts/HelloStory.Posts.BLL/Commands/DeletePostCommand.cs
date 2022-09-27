using HelloStory.Shared.BLL.Interfaces;

namespace HelloStory.Posts.BLL.Commands;

public readonly record struct DeletePostCommand(Guid Id): IHttpRequest;