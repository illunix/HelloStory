using HumanExpBook.DAL.Entities.Abstract;

namespace HumanExpBook.DAL.Entities;

public sealed record Comment(
    Guid PostId,
    Guid UserId,
    Guid? ParentCommentId,
    string Content
) : EntityBase
{
    public bool IsEdited { get; set; }
    public DateTime CreatedAt { get; }
    public User? User { get; init; }
}