using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Authflow;

public readonly record struct RefreshAccessTokenCommand(
    Guid CurrentUserId,
    string RefreshToken
) : IHttpRequest;