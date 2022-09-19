using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Comments;

public readonly record struct CreateCommentCommand(
    Guid PostId,
    Guid CurrentUserId,
    Guid? ParentCommentId,
    string Content
) : IHttpRequest;