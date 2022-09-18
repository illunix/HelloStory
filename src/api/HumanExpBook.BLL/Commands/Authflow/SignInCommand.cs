using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Authflow;

public readonly record struct SignInCommand(
    string EmailOrUsername,
    string Password
) : IHttpRequest;