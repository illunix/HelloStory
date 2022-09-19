﻿using HumanExpBook.BLL.Interfaces;

namespace HumanExpBook.BLL.Commands.Comments;

public readonly record struct UpdateCommentCommand(
    Guid Id,
    string Content
) : IHttpRequest;