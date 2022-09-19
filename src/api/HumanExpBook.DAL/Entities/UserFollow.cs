using HumanExpBook.DAL.Entities.Abstract;

namespace HumanExpBook.DAL.Entities;

public sealed record UserFollow : EntityBase
{
    public Guid UserId { get; init; }
    public string? Hashtag { get; init; }
}