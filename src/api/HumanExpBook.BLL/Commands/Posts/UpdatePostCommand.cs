using HumanExpBook.BLL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumanExpBook.BLL.Commands.Posts;

public readonly record struct UpdatePostCommand(
    Guid Id,
    string Content
) : IHttpRequest;