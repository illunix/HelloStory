﻿using HumanExpBook.DAL.Entities.Abstract;

namespace HumanExpBook.DAL.Entities;

public sealed record Post(
    Guid UserId,
    string Content
) : EntityBase
{
    public bool Edited { get; set; }
    public DateTime CreatedAt { get; }
    public User? User { get; init; }
}