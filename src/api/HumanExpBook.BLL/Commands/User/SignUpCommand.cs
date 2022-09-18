using HumanExpBook.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanExpBook.BLL.Commands.User;

public readonly record struct SignUpCommand(
    string Email,
    string Username,
    string Password,
    string ConfirmPassword
) : IHttpRequest;