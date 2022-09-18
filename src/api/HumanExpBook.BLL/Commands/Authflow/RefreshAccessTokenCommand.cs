using HumanExpBook.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanExpBook.BLL.Commands.Authflow;

public readonly record struct RefreshAccessTokenCommand(string RefreshToken) : IHttpRequest
{
    public string? CurrentUserId { get; init; }
}