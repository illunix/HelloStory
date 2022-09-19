using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Comments;

public readonly record struct DeleteCommentCommand(Guid Id) : IHttpRequest;