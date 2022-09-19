using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Authflow;

public readonly record struct RevokeRefreshTokenCommand(Guid CurrentUserId) : IHttpRequest;