using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Posts;

public readonly record struct DeletePostCommand(Guid Id) : IHttpRequest;